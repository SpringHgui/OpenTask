// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTask.Core.Base;
using System.Threading;
using System.Threading.Tasks;

namespace OpenTask.Core.Services;

public class OpenTaskWorkerBackgroundService : BackgroundService
{
    private readonly ILogger<OpenTaskWorkerBackgroundService> _logger;
    private readonly TaskWorker taskWorker;

    public OpenTaskWorkerBackgroundService(ILogger<OpenTaskWorkerBackgroundService> logger, TaskWorker jobExecutor)
    {
        _logger = logger;
        taskWorker = jobExecutor;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await taskWorker.RunAsync(stoppingToken);
    }
}
