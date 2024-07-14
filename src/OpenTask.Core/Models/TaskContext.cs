// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using System.Threading;

namespace OpenTask.Core.Models
{
    public class TaskContext
    {
        public TaskContext(TodoTask task)
        {
            TaskInfo = task;
        }

        ///// <summary>
        ///// 取消任务的令牌
        ///// </summary>
        //public CancellationToken CancellationToken { get; internal set; } = CancellationToken.None;

        /// <summary>
        /// 任务信息
        /// </summary>
        public TodoTask TaskInfo { get; }
    }
}
