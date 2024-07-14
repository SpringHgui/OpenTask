// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using MQTTnet;
using MQTTnet.Client;
using System.Collections.Concurrent;

namespace OpenTask.Application.Core.Interface
{
    public interface ITaskServer
    {
        string ExternalUrl { get; }

        string Identifier { get; }

        IDiscovery Discovery { get; }

        ConcurrentDictionary<string, ExecutorClient> CurrentNodeOnlineUsers { get; }

        ConcurrentDictionary<string, IEnumerable<ExecutorClient>> OtherNodeOlineUsers { get; }

        IEnumerable<ExecutorClient> GetAllClientsOnline();

        IEnumerable<ExecutorClient> GetClientsByAppName(string appid);

        Task<MqttClientPublishResult> PublishAsync(MqttApplicationMessage applicationMessage, CancellationToken cancellationToken = default);

        Task StartAsync(CancellationToken stoppingToken);

        void StartDispatch();

        void StopDispatch();
    }
}
