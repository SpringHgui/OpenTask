// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OpenTask.Application.Services
{
    public class CleanerBackgroundService : BackgroundService
    {
        private readonly ILogger<CleanerBackgroundService> logger;

        public CleanerBackgroundService(ILogger<CleanerBackgroundService> logger)
        {
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //logger.LogInformation($"now: {DateTime.Now.ToTimestamp()}");
                await Task.Delay(1000);
            }
        }
    }
}
