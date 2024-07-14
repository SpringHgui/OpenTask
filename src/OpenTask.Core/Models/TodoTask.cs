// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

namespace OpenTask.Core.Models
{
    public class TodoTask
    {
        /// <summary>
        /// 调度实例id
        /// </summary>
        public long TaskId { get; set; }

        /// <summary>
        /// 任务唯一标识
        /// </summary>
        public long JobId { get; set; }

        /// <summary>
        /// 客户端分组名，指定的分组客户端才会执行本任务
        /// </summary>
        public string? GroupName { get; set; }

        /// <summary>
        /// 任务名
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 任务handler
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 时间类型 cron
        /// </summary>
        public string? TimeType { get; set; }

        /// <summary>
        /// 时间表达式
        /// </summary>
        public string? TimeExpression { get; set; }

        /// <summary>
        /// 重试间隔 单位s
        /// </summary>
        public int AttemptInterval { get; set; }

        /// <summary>
        /// 失败重试次数
        /// </summary>
        public int MaxAttempt { get; set; }

        /// <summary>
        /// 执行模式 alone broadcast
        /// </summary>
        public string ExecuteMode { get; set; } = null!;

        /// <summary>
        /// 执行参数
        /// </summary>
        public string? JobParams { get; set; }
    }
}
