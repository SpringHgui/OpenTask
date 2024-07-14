// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.AspNetCore.Mvc.Testing;

namespace OpenTask.WebApi.Tests;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _ = builder.ConfigureServices(services =>
        {
            _ = services.AddTransient<IStartupFilter, FakeUserStartupFilter>();
        });

        _ = builder.ConfigureAppConfiguration(config =>
        {
        });

        _ = builder.UseEnvironment("Development");
    }

    protected override void ConfigureClient(HttpClient client)
    {
        base.ConfigureClient(client);
    }
}
