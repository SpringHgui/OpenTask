// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.Extensions.Hosting;
using OpenTask.Application.Core.Interface;

namespace OpenTask.Application.Services
{
    public class OpenTaskServerBackgroundService : BackgroundService
    {
        private readonly ITaskServer server;

        public OpenTaskServerBackgroundService(ITaskServer server)
        {
            this.server = server;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await server.StartAsync(stoppingToken);
        }
    }
}
