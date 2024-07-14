// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenTask.Application.Base;
using OpenTask.Application.Clients;

namespace OpenTask.WebApi.Controllers
{
    public class ClientController : BaseController
    {
        public ClientController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<BaseResponse<ListClientsResponse>> ListClients([FromQuery] ListClientsRequest request)
        {
            return await RequestAsync<ListClientsRequest, ListClientsResponse>(request);
        }
    }
}