// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.Extensions.DependencyInjection;
using MQTTnet.Diagnostics;
using OpenTask.Application.Core;
using OpenTask.Application.Core.Interface;
using OpenTask.Application.Services;

namespace OpenTask.Application.Extensions
{
    public static class OpentaskExtensions
    {
        public static IServiceCollection AddOpenTaskServer(this IServiceCollection services)
        {
            _ = services.AddTransient<IMqttNetLogger, MyLog>();
            _ = services.AddTransient<IDiscovery, DiscoveryFromDb>();
            _ = services.AddTransient<ITaskDispatcher, TaskDispatcher>();
            _ = services.AddTransient<ITaskSchedulingHandler, SchedulingHandler>();

            // Server是单例的，方便全局调用
            _ = services.AddSingleton<ITaskServer, OpenTaskServer>();
            _ = services.AddHostedService<OpenTaskServerBackgroundService>();

            return services;
        }

    }
}
