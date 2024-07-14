// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using MediatR;
using OpenTask.Application.Core.Interface;

namespace OpenTask.Application.Clients
{
    public class ListClientsHandler : IRequestHandler<ListClientsRequest, ListClientsResponse>
    {
        private readonly ITaskServer serverSystem;
        public ListClientsHandler(ITaskServer serverSystem)
        {
            this.serverSystem = serverSystem;
        }

        Task<ListClientsResponse> IRequestHandler<ListClientsRequest, ListClientsResponse>.Handle(ListClientsRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<ExecutorClient> list = serverSystem.GetAllClientsOnline();
            return Task.FromResult(new ListClientsResponse
            {
                Count = list.Count(),
                Rows = list.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize),
            });
        }


    }
}
