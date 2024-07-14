// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.WindowsServices;
using OpenTask.Application;
using OpenTask.Application.Base;
using OpenTask.Application.Extensions;
using OpenTask.Application.Lockers;
using OpenTask.Persistence;
using OpenTask.Persistence.Contexts;
using OpenTask.Persistence.Extensions;
using OpenTask.Utility.Converters;
using OpenTask.WebApi.Extensions;
using OpenTask.WebApi.Filters;
using Serilog;
using Serilog.Events;
using System.Diagnostics;
using System.Text.Json.Serialization;

const string outputTemplate = "[{Timestamp:HH:mm:ss.fff} {TraceId} {Level:u3}] {Message:lj}{NewLine}{Exception}";
if (WindowsServiceHelpers.IsWindowsService())
{
    // 调用 SetCurrentDirectory 并使用应用的发布位置路径。 
    // 不要调用 GetCurrentDirectory 来获取路径，因为在调用 GetCurrentDirectory 时，Windows 服务应用将返回 C:\WINDOWS\system32 文件夹。 
    // 有关详细信息，请参阅当前目录和内容根部分。 请先执行此步骤，然后再在 CreateWebHostBuilder 中配置应用。
    var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
    var pathToContentRoot = Path.GetDirectoryName(pathToExe);
    Directory.SetCurrentDirectory(pathToContentRoot);
}

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog((config) =>
{
    _ = config.MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information) // Microsoft.Hosting.Lifetime
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
        .WriteTo.File("logs/.log", outputTemplate: outputTemplate, rollingInterval: RollingInterval.Hour, shared: true)
        .WriteTo.Console(outputTemplate: outputTemplate);
});

builder.Services.AddHealthChecks();

// Add services to the container.
builder.Services.AddControllers().ConfigureApiBehaviorOptions(delegate (ApiBehaviorOptions options)
{
    options.InvalidModelStateResponseFactory = delegate (ActionContext context)
    {
        IEnumerable<KeyValuePair<string, string[]>> errors = context.ModelState.Select((x) => new KeyValuePair<string, string[]>(x.Key, x!.Value!.Errors.ToArray().Select(x => x.ErrorMessage).ToArray()));
        JsonResult res = new(new BaseResponse<IEnumerable<KeyValuePair<string, string[]>>>()
        {
            Code = StatusCodes.Status400BadRequest,
            Message = errors.FirstOrDefault().Value.FirstOrDefault() ?? "参数不合法",
            TraceId = context.HttpContext.TraceIdentifier,
            Result = errors
        })
        {
            StatusCode = StatusCodes.Status200OK
        };
        return res;
    };
}).AddJsonOptions(options =>
{
    //options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.Converters.Add(new JsonLongConverter());
    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
    options.JsonSerializerOptions.NumberHandling
        = JsonNumberHandling.AllowReadingFromString;
});

builder.Services.AddWindowsService();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// 认证
builder.Services.AddJwtAuthentication(builder.Configuration);

// Swagger文档
builder.Services.AddSwagger();

// cqrs&eventbus
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<MediatRProfile>());

// 分布式缓存
builder.Services.AddDistributedMemoryCache();

// 注入默认持久化实现
builder.Services.AddDefalutPersistence(builder.Configuration);

// OpenTask服务
builder.Services.AddOpenTaskServer();

builder.Services.AddTransient<LockerService>();

builder.Services.AddSingleton<CustomExceptionFilterAttribute>();

builder.Services.AddCors((options) =>
{
    options.AddPolicy("default", policy =>
    {
        _ = policy.WithOrigins("http://127.0.0.1:5173")
            .WithOrigins("127.0.0.1:5173")
            .WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader().AllowCredentials();
    });
});

WebApplication app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var openTaskContext = scope.ServiceProvider.GetRequiredService<OpenTaskContext>();
    if (openTaskContext.Database.GetPendingMigrations().Any())
    {
        openTaskContext.Database.Migrate();
    }
};

if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseVueRouterHistory();

app.MapHealthChecks("/healthz");

app.UseCors("default");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }