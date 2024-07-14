// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

namespace OpenTask.Application.Core.Models
{
    public class TaskServerOptions
    {
        /// <summary>
        /// MQTT server 监听IP
        /// </summary>
        public string? Ip { get; set; }

        /// <summary>
        /// MQTT server 监听端口
        /// </summary>
        public int Port { get; set; } = 1883;

        /// <summary>
        /// MQTT server 外部访问地址
        /// </summary>
        public string? ExternalUrl { get; set; }
    }
}
