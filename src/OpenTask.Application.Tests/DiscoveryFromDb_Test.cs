// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using OpenTask.Application.Core;

namespace OpenTask.Application.Tests
{
    public class DiscoveryFromDb_Test
    {
        private readonly DiscoveryFromDb discovery;

        [Obsolete]
        public DiscoveryFromDb_Test()
        {
            Mock<ILogger<DiscoveryFromDb>> logger = new();

            ServiceCollection service = new();
            ServiceProvider provider = service.BuildServiceProvider();

            discovery = new DiscoveryFromDb(provider, logger.Object);
        }


        [Fact]
        public void CheckSlot_IsWholeSlot()
        {
            IList<Domain.Servers.OpenTaskServer> mqttNodes = [];
            for (int i = 0; i < 10; i++)
            {
                mqttNodes.Add(new Domain.Servers.OpenTaskServer
                {
                    ServerId = Guid.NewGuid().ToString(),
                });
            }

            IEnumerable<Domain.Servers.OpenTaskServer> nodes = discovery.CalculateSlot(mqttNodes);

            bool ok = discovery.IsWholeSlot(nodes);

            Assert.True(ok);
        }


        [Fact]
        public void CheckSlot_Is_Not_WholeSlot()
        {
            IList<Domain.Servers.OpenTaskServer> mqttNodes = [];
            int count = 10;
            for (int i = 0; i < count; i++)
            {
                mqttNodes.Add(new Domain.Servers.OpenTaskServer
                {
                    ServerId = Guid.NewGuid().ToString(),
                });
            }

            IEnumerable<Domain.Servers.OpenTaskServer> nodes = discovery.CalculateSlot(mqttNodes);

            bool ok = discovery.IsWholeSlot(nodes.TakeLast(count - 1));

            Assert.False(ok);

            bool ok1 = discovery.IsWholeSlot(nodes.Take(count - 1));
            Assert.False(ok1);

            bool ok2 = discovery.IsWholeSlot(nodes.Skip(1).Take(count - 1));
            Assert.False(ok2);
        }

        [Fact]
        public void CheckSlot_IsWholeSlot_When_One_Node()
        {
            IList<Domain.Servers.OpenTaskServer> mqttNodes = [];
            for (int i = 0; i < 1; i++)
            {
                mqttNodes.Add(new Domain.Servers.OpenTaskServer
                {
                    ServerId = Guid.NewGuid().ToString(),
                });
            }

            IEnumerable<Domain.Servers.OpenTaskServer> nodes = discovery.CalculateSlot(mqttNodes);

            bool ok = discovery.IsWholeSlot(nodes);

            Assert.True(ok);
        }


        [Fact]
        public void CheckSlot_Is_Not_WholeSlot_When_One_Node()
        {
            IList<Domain.Servers.OpenTaskServer> mqttNodes = [];
            int count = 1;
            for (int i = 0; i < count; i++)
            {
                mqttNodes.Add(new Domain.Servers.OpenTaskServer
                {
                    ServerId = Guid.NewGuid().ToString(),
                });
            }

            IEnumerable<Domain.Servers.OpenTaskServer> nodes = discovery.CalculateSlot(mqttNodes);

            bool ok = discovery.IsWholeSlot(nodes.TakeLast(count - 1));

            Assert.False(ok);

            bool ok1 = discovery.IsWholeSlot(nodes.Take(count - 1));
            Assert.False(ok1);

            bool ok2 = discovery.IsWholeSlot(nodes.Skip(1).Take(count - 1));
            Assert.False(ok2);
        }
    }
}
