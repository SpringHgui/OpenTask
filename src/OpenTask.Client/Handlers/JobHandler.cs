// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using OpenTask.Core.Base;
using OpenTask.Core.Models;

namespace OpenTask.Client.Handlers
{
    internal class JobHandler : ITaskHandler
    {
        public Task RunAsync(TaskContext context)
        {
            Console.WriteLine("Job");
            Thread.Sleep(20000);

            return Task.CompletedTask;
        }
    }
}
