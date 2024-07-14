// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.Extensions.Hosting;
using OpenTask.Client.Handlers;
using OpenTask.Core.Extensions;
using Serilog;

namespace OpenTask.Client
{
    internal class Program
    {
        private const string outputTemplate = "[{Timestamp:HH:mm:ss.fff} {TraceId} {Level:u3}] {Message:lj}{NewLine}{Exception}";
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((ctx, services) =>
            {
                _ = services.AddOpenTaskWorker(ctx.Configuration.GetSection("OpenTaskWorker"), options =>
                {
                    options.AddHandler<DemoJobHandler>();
                    options.AddHandler<JobHandler>();
                });
            }).UseSerilog((context, configuration) =>
            {
                _ = configuration.WriteTo.Console(Serilog.Events.LogEventLevel.Debug, outputTemplate: outputTemplate);
            })
            .Build();

            host.Run();
        }
    }
}