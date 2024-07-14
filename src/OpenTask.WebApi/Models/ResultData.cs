// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

namespace OpenTask.WebApi.Models
{
    public class ResultData
    {
        public ResultData()
        {
            success = true;
        }

        public bool success { get; set; }

        public string message { get; set; }


        public object data { get; set; }

        /// <summary>
        /// 链路追踪标识
        /// </summary>
        public string trace_id { get; set; }
    }
}
