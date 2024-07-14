// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using OpenTask.Persistence.Contexts;
using OpenTask.Persistence.Models;
using Org.BouncyCastle.Security;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;

namespace OpenTask.Persistence.Extensions
{
    public static class PersistenceCollectionExtensions
    {
        public static IServiceCollection AddDefalutPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            using var provider = services.BuildServiceProvider();
            var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("PersistenceCollectionExtensions");

            string? constr = configuration["ConnectionStrings:Core"];
            if (constr == null)
            {
                throw new InvalidParameterException("未配置数据库连接字符串");
            }

            logger.LogInformation($"db: {Regex.Replace(constr, @"(?<=password=).*?(?=;|$)", "******")}");

            _ = services.AddDapperdbContext<DapperdbContext>(options =>
            {
                string? constr = configuration["ConnectionStrings:Core"];
                if (string.IsNullOrEmpty(constr))
                {
                    throw new Exception("数据库连接字符串未配置");
                }

                options.Configuration = constr;
            });

            services.AddDbContext<OpenTaskContext>(options =>
            {
                if (constr == null)
                {
                    throw new Exception("未找到数据库连接配置");
                }

                options.UseMySQL(constr);
            });

            services.AddAssembly("OpenTask.Persistence");

            return services;
        }


        /// <summary> 
        /// 获取程序集
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static Assembly GetAssemblyByName(string assemblyName)
        {
            return AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName));
        }

        /// <summary>
        /// 程序集依赖注入
        /// </summary>
        /// <param name="services">服务实例</param>
        /// <param name="assemblyName">程序集名称。不带DLL</param>
        public static void AddAssembly(
            this IServiceCollection services, string assemblyName)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException(nameof(assemblyName));
            }

            var assembly = GetAssemblyByName(assemblyName);
            if (assembly == null)
            {
                throw new DllNotFoundException(nameof(assembly));
            }

            // 获取所有类（不包含泛型集合以及抽象类）
            var list = assembly.GetTypes().Where(o =>
                o.IsClass && !o.IsAbstract
                && !o.IsGenericType
                && (!o.Namespace?.StartsWith("Microsoft") ?? false)
                && (!o.Namespace?.StartsWith("System") ?? false));

            foreach (var type in list)
            {
                // 获取定义接口
                var injectAttr = type.GetCustomAttribute<InjectAttribute>();
                if (injectAttr == null)
                {
                    continue;
                }

                var interfacesList = type.GetInterfaces();
                if (!interfacesList.Any())
                {
                    interfacesList = new Type[1] { type };
                }

                var innerLifetime = injectAttr.GetServiceLifetime();
                foreach (Type serviceType in interfacesList)
                {
                    switch (innerLifetime)
                    {
                        case ServiceLifetime.Singleton:
                            services.TryAddSingleton(serviceType, type);
                            break;
                        case ServiceLifetime.Scoped:
                            services.TryAddScoped(serviceType, type);
                            break;
                        case ServiceLifetime.Transient:
                            services.TryAddTransient(serviceType, type);
                            break;
                    }
                }
            }
        }
    }
}
