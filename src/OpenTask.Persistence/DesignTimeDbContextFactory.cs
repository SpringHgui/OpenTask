// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTask.Persistence.Contexts;

namespace OpenTask.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<OpenTaskContext>
    {
        public OpenTaskContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OpenTaskContext>();
            optionsBuilder.UseMySQL("Data Source=opentask.db");

            return new OpenTaskContext(optionsBuilder.Options);
        }
    }
}
