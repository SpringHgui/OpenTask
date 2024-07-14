// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTask.Application.Base;
using OpenTask.Persistence.Models;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.Json;

namespace OpenTask.WebApi.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            _ = services.AddSwaggerGen(options =>
            {
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = JwtOptions.DefaultScheme,
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });

                options.SupportNonNullableReferenceTypes();
                options.AddSecurityDefinition(JwtOptions.DefaultScheme, new OpenApiSecurityScheme
                {
                    Description = "Value Bearer {token}",
                    Name = "Authorization",//jwtĬ�ϵĲ�������
                    In = ParameterLocation.Header,//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
                    Type = SecuritySchemeType.ApiKey
                });

                options.ParameterFilter<MyParameterFilter>();
                options.SchemaFilter<MySchemaFilter>();
                options.CustomOperationIds(apiDesc =>
                {
                    return (apiDesc.ActionDescriptor as ControllerActionDescriptor)!.ActionName; // apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name.Replace("Async", string.Empty) : null;
                });
            });

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfigurationRoot configuration)
        {
            _ = services.AddOptions<JwtOptions>().Bind(configuration.GetSection(JwtOptions.Jwt)).ValidateDataAnnotations().ValidateOnStart();

            JwtOptions jwtOptions = new();
            configuration.GetSection(JwtOptions.Jwt).Bind(jwtOptions);

            _ = services.AddAuthentication(JwtOptions.DefaultScheme).AddJwtBearer(JwtOptions.DefaultScheme, null, delegate (JwtBearerOptions options)
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(Convert.ToInt32(jwtOptions.ClockSkew)),
                    ValidateIssuerSigningKey = true,
                    ValidAudience = jwtOptions.ValidAudience,
                    ValidIssuer = jwtOptions.ValidIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtOptions.SigningKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async delegate (JwtBearerChallengeContext context)
                    {
                        context.HandleResponse();

                        context.Response.ContentType = "application/json;charset=utf-8";
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new BaseResponse<string>()
                        {
                            Result = context.Error,
                            Code = StatusCodes.Status401Unauthorized,
                            TraceId = context.HttpContext.TraceIdentifier,
                            Message = context.Error ?? "未登录",
                        }, new JsonSerializerOptions(JsonSerializerDefaults.Web)));
                    },
                };
            });

            return services;
        }

        /// <summary> 
        /// 获取程序集
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static Assembly GetAssemblyByName(string assemblyName)
        {
            return AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName));
        }

        /// <summary>
        /// 程序集依赖注入
        /// </summary>
        /// <param name="services">服务实例</param>
        /// <param name="assemblyName">程序集名称。不带DLL</param>
        public static void AddAssembly(
            this IServiceCollection services, string assemblyName)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException(nameof(assemblyName));
            }

            var assembly = GetAssemblyByName(assemblyName);
            if (assembly == null)
            {
                throw new DllNotFoundException(nameof(assembly));
            }

            // 获取所有类（不包含泛型集合以及抽象类）
            var list = assembly.GetTypes().Where(o =>
                o.IsClass && !o.IsAbstract
                && !o.IsGenericType
                && (!o.Namespace?.StartsWith("Microsoft") ?? false)
                && (!o.Namespace?.StartsWith("System") ?? false));

            foreach (var type in list)
            {
                // 获取定义接口
                var injectAttr = type.GetCustomAttribute<InjectAttribute>();
                if (injectAttr == null)
                {
                    continue;
                }

                var interfacesList = type.GetInterfaces();
                if (!interfacesList.Any())
                {
                    interfacesList = new Type[1] { type };
                }

                var innerLifetime = injectAttr.GetServiceLifetime();
                foreach (Type serviceType in interfacesList)
                {
                    switch (innerLifetime)
                    {
                        case ServiceLifetime.Singleton:
                            services.TryAddSingleton(serviceType, type);
                            break;
                        case ServiceLifetime.Scoped:
                            services.TryAddScoped(serviceType, type);
                            break;
                        case ServiceLifetime.Transient:
                            services.TryAddTransient(serviceType, type);
                            break;
                    }
                }
            }
        }
    }
}
