// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using System.ComponentModel;

namespace OpenTask.Domain.TaskInfos
{
    [Flags]
    public enum TaskLogStatus : int
    {
        [Description("初始化")]
        Inited = 0,

        [Description("执行中")]
        Process = 1,

        [Description("成功")]
        SUCCESS = 2,

        [Description("失败")]
        FAIL = 3,
    }
}
