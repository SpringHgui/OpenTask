// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H


using System;
using System.ComponentModel.DataAnnotations;

namespace OpenTask.Core.Models
{
    public class OpenWorkerOptions
    {
        [Required]
        public string[]? Addr { get; set; }

        public string? ClientId { get; set; }

        [Required]
        public string? AppName { get; set; }

        public string? Token { get; set; }

        internal void Validate()
        {
            if (Addr == null)
            {
                throw new ArgumentNullException("配置 Addr 不可为空");
            }

            if (Addr.Length > 1)
            {
                throw new ArgumentNullException("Addr 尚不支持配置多个地址，请删除多余的");
            }

            foreach (var item in Addr)
            {
                // 检查地址格式
            }
        }
    }
}
