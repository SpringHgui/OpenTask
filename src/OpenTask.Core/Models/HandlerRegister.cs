// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.Extensions.DependencyInjection;
using OpenTask.Core.Base;
using System;
using System.Collections.Generic;

namespace OpenTask.Core.Models
{
    public class HandlerRegister
    {
        private readonly IServiceCollection services;

        public HandlerRegister(IServiceCollection services)
        {
            this.services = services;
        }

        internal Dictionary<string, Type> handlers = [];

        public void AddHandler<T>()
             where T : class, ITaskHandler
        {
            _ = services.AddTransient<T>();
            handlers.Add(typeof(T).Name, typeof(T));
        }
    }
}
