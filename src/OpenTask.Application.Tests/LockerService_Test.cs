// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OpenTask.Application.Lockers;
using OpenTask.Domain.Lockers;
using OpenTask.Persistence;
using OpenTask.Persistence.Extensions;

namespace OpenTask.Application.Tests
{
    public class LockerService_Test
    {
        private readonly ServiceProvider provider;
        public LockerService_Test()
        {
            Mock<IConfiguration> configurationMock = new();
            _ = configurationMock.Setup(c => c["ConnectionStrings:Core"]).Returns("server=101.37.166.120;Port=31002;user id=root;database=open_task;pooling=true;password=OPEN_TASK_!@#");

            ServiceCollection service = new();

            _ = service.AddAutoMapper(typeof(AutoMapperProfile));
            _ = service.AddDefalutPersistence(configurationMock.Object);
            _ = service.AddTransient<LockerService>();

            provider = service.BuildServiceProvider();
        }

        [Fact]
        public void BaseTtest()
        {
            string key = "test";

            for (int i = 0; i < 10; i++)
            {
                int success = 0;
                string lockBy = string.Empty;
                ParallelLoopResult res = Parallel.For(0, 100, (index) =>
                {
                    IServiceScope scope = provider.CreateScope();
                    LockerService lockerService = scope.ServiceProvider.GetRequiredService<LockerService>();
                    Assert.NotNull(lockerService);

                    if (lockerService.TryLock(key, index.ToString(), 60, out Locker? locker))
                    {
                        success++;
                        if (!string.IsNullOrEmpty(lockBy))
                        {
                            Assert.Equal(lockBy, index.ToString());
                        }
                        else
                        {
                            lockBy = index.ToString();
                        }
                    }
                    else
                    {

                    }
                });

                Assert.Equal(1, success);
            }
        }
    }
}
