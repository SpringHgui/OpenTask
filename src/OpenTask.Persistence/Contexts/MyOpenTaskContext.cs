﻿// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.EntityFrameworkCore;
using OpenTask.Persistence.Entitys;
using OpenTask.Utility.Helpers;

namespace OpenTask.Persistence.Contexts;

public partial class OpenTaskContext : DbContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<OtUser>()
            .HasData(new OtUser
            {
                Id = 1,
                UserName = "admin",
                Password = Md5Helper.MD5Encrypt64("OpenTask"),
                CreatedAt = DateTime.Now
            });
    }
}

