// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using System.Security.Claims;

namespace OpenTask.WebApi.Tests
{
    public class FakeUserStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                _ = app.Use(async (context, next) =>
                {
                    var claims = new Claim[] { new Claim(ClaimTypes.Name, "test") };

                    var claimsIdentity = new ClaimsIdentity(claims, "Basic");
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    context.User = claimsPrincipal;

                    await next();
                });

                next(app);
            };
        }
    }

}
