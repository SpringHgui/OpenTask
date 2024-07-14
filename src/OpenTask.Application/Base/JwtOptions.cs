// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using System.ComponentModel.DataAnnotations;

namespace OpenTask.Application.Base
{
    public class JwtOptions
    {
        public const string Jwt = "Jwt";
        public const string DefaultScheme = "Bearer";

        [Required]
        public string SigningKey { get; set; } = null!;

        public int ClockSkew { get; set; }

        [Required]
        public string ValidAudience { get; set; } = null!;

        [Required]
        public string ValidIssuer { get; set; } = null!;

        [Range(1, int.MaxValue)]
        public int Expires { get; set; }
    }
}
