// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using System.Collections.Concurrent;

namespace OpenTask.Application.Core.Interface
{
    public interface IDiscovery
    {
        public Task StartAsync(ITaskServer myMqttServer, CancellationToken stoppingToken);

        public event OnSlotParamer OnSloting;

        public event OnSlotParamer OnSloted;

        public delegate Task OnNodeSlotsChange(int start, int end);

        public delegate Task OnNewNodeChange(Domain.Servers.OpenTaskServer node);

        public delegate Task OnSlotParamer(IDiscovery sender, IEnumerable<Domain.Servers.OpenTaskServer> mqttNodes);

        public bool IsWholeSlot(IEnumerable<Domain.Servers.OpenTaskServer> latestNodes);

        public IEnumerable<Domain.Servers.OpenTaskServer> CalculateSlot(IEnumerable<Domain.Servers.OpenTaskServer> latestNodes);

        public ConcurrentDictionary<string, ClusterSubscriber> clusterSubscribers { get; }

        public IEnumerable<Domain.Servers.OpenTaskServer> FindAllOnlineServer();
    }
}
