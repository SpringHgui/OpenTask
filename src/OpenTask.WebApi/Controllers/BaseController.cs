// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenTask.Application.Base;
using OpenTask.Domain.Users;
using OpenTask.WebApi.Filters;
using System.Text.Json;

namespace OpenTask.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ServiceFilter(typeof(CustomExceptionFilterAttribute))]
    public class BaseController : ControllerBase
    {
        protected readonly IMediator mediator;

        public BaseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [NonAction]
        protected async Task<BaseResponse<TResult>> RequestAsync<TCommand, TResult>(TCommand command)
            where TCommand : IRequest<TResult>
        {
            return new BaseResponse<TResult>
            {
                TraceId = Request.HttpContext.TraceIdentifier,
                Code = StatusCodes.Status200OK,
                Message = "ok",
                Result = await mediator.Send(command)
            };
        }

        public CurrentUser? CurrentUser
        {
            get
            {
                System.Security.Claims.Claim? user = Request.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "User");
                return user != null ? JsonSerializer.Deserialize<CurrentUser>(user.Value) : throw new Exception("解析用户认证信息失败");
            }
        }

        //[NonAction]
        //public virtual BaseResponse Fail<T>(string msg, Dictionary<string, string[]> erros)
        //{
        //    return new ErrorInfo(msg, erros, Request.HttpContext.TraceIdentifier);
        //}
    }
}
