// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Data;

namespace OpenTask.Persistence.Extensions
{
    public class DapperdbContext
    {
        protected DapperdbContextOptions options;

        public DapperdbContext(IOptions<DapperdbContextOptions> optionsAccessor)
        {
            options = optionsAccessor.Value;
        }

        public IDbConnection CreateConnection() => new MySqlConnection(options.Configuration);
    }
}
