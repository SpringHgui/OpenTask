// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTask.Core.Base;
using OpenTask.Core.Models;
using OpenTask.Core.Services;
using System;

namespace OpenTask.Core.Extensions
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// 添加工作节点任务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="section"></param>
        /// <param name="configHandler"></param>
        /// <returns></returns>
        public static IServiceCollection AddOpenTaskWorker(this IServiceCollection services, IConfigurationSection section, Action<HandlerRegister> configHandler)
        {
            _ = services.AddOptions<OpenWorkerOptions>()
                .Bind(section).ValidateDataAnnotations().ValidateOnStart();

            HandlerRegister register = new(services);
            configHandler.Invoke(register);
            _ = services.AddSingleton(register);
            //_ = services.AddSingleton<IMqttNetLogger, MyLog>();
            _ = services.AddSingleton<TaskWorker>();
            _ = services.AddHostedService<OpenTaskWorkerBackgroundService>();


            return services;
        }
    }
}
