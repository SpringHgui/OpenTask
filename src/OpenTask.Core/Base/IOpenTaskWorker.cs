// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using OpenTask.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace OpenTask.Core.Base
{
    public interface IOpenTaskWorker
    {
        Task DoTaskAsync(OnTaskRequest job);

        Task RunAsync(CancellationToken cancellationToken);
    }
}
