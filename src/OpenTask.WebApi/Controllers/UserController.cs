// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenTask.Application.Base;
using OpenTask.Application.User.Login;

namespace OpenTask.WebApi.Controllers
{
    public class UserController : BaseController
    {
        public UserController(IMediator mediator) : base(mediator)
        {
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<BaseResponse<LoginResponse>> Login(LoginCommand request)
        {
            return await RequestAsync<LoginCommand, LoginResponse>(request);
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public ResultData GetToken([FromServices] UserService userService, GetTokenRequest request)
        //{
        //    mediator.Send
        //    if (!userService.CheckPassWord(request.UserName, request.PassWord))
        //    {
        //        throw new Exception("用户名或密码错误");
        //    }

        //    var user = userService.GetUserByUserName(request.UserName);
        //    if (user == null)
        //    {
        //        throw new Exception("获取用户失败");
        //    }

        //    var claims = new Claim[] {
        //        new Claim("USER", System.Text.Json.JsonSerializer.Serialize(user)),
        //    };

        //    var token = GenerateToken(claims, 240, configuration);
        //    ResultData.data = "Bearer " + token;

        //    return ResultData;
        //}

        //protected string GenerateToken(IEnumerable<Claim> claims, int expiresMinutes, IConfiguration configuration)
        //{
        //    var Secret = configuration.GetSection("JWT")["IssuerSigningKey"];
        //    var issuer = configuration.GetSection("JWT")["ValidIssuer"];
        //    var audience = configuration.GetSection("JWT")["ValidAudience"];

        //    var key = new SymmetricSecurityKey(Convert.FromBase64String(Secret));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var securityToken = new JwtSecurityToken(
        //        issuer: issuer,
        //        audience: audience,
        //        claims: claims,
        //        expires: DateTime.Now.AddMinutes(expiresMinutes),
        //        signingCredentials: creds);

        //    return new JwtSecurityTokenHandler().WriteToken(securityToken);
        //}

    }
}
