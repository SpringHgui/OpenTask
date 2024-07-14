// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OpenTask.Application.Base;

namespace OpenTask.WebApi.Filters
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<CustomExceptionFilterAttribute> _logger;

        public CustomExceptionFilterAttribute(ILogger<CustomExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "【全局异常捕获】");
            BaseResponse<object> res = new()
            {
                TraceId = context.HttpContext.TraceIdentifier,
                Result = null,
                Message = context.Exception.Message,
                Code = StatusCodes.Status500InternalServerError,
            };

            JsonResult result = new(res) { StatusCode = StatusCodes.Status200OK };

            context.Result = result;
            context.ExceptionHandled = true;
        }
    }
}
