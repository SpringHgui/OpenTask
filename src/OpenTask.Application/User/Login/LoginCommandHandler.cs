// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.Extensions.Options;
using OpenTask.Application.Base;
using OpenTask.Application.Base.Commands;
using OpenTask.Domain.Users;

namespace OpenTask.Application.User.Login
{
    public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly JwtOptions jwtOptions;

        public LoginCommandHandler(IUserRepository userRepository, IOptions<JwtOptions> options)
        {
            this.userRepository = userRepository;
            jwtOptions = options.Value;
        }

        public Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            Domain.Users.User? user = userRepository.FindByUserName(request.UserName);
            if (user == null)
            {
                throw new Exception("账号或密码错误");
            }

            bool success = user.CheckPass(request.Password);
            if (!success)
            {
                throw new Exception("账号或密码错误");
            }

            string token = user.GenToken(jwtOptions.SigningKey);

            return Task.FromResult(new LoginResponse
            {
                Token = token,
            });
        }
    }
}
