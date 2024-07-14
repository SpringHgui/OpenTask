// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Dommel;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using OpenTask.Persistence.Base;

namespace OpenTask.Persistence.Extensions
{
    public static class DapperdbContextServiceCollectionExtensions
    {
        public static IServiceCollection AddDapperdbContext<T>(this IServiceCollection services, Action<DapperdbContextOptions> setupAction)
            where T : DapperdbContext
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            // 表名
            DommelMapper.SetTableNameResolver(new CustomTableNameResolver());
            // 主键
            //DommelMapper.SetKeyPropertyResolver(new CustomKeyPropertyResolver());
            // 字段
            DommelMapper.SetColumnNameResolver(new CustomColumnNameResolver());

            // DommelMapper.AddSqlBuilder(typeof(MySqlConnection), new MySqlSqlBuilder());

            //DommelMapper.LogReceived = (sql) =>
            //{
            //    logger.LogInformation(sql);
            //};

            _ = services.Configure(setupAction);
            _ = services.AddScoped<T>();
            return services;
        }
    }
}
