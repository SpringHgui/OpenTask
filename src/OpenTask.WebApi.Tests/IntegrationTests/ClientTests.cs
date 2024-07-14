// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.AspNetCore.Mvc.Testing;
using OpenTask.Application.Base;
using OpenTask.Application.User.Login;

namespace OpenTask.WebApi.Tests.IntegrationTests;

public class ClientTests
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient client;

    public ClientTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        client = _factory.CreateClient();
    }

    [Theory]
    [InlineData("/api/Client/ListClients")]
    public async Task Get_Clients(string url)
    {
        HttpResponseMessage response = await client.GetAsync(url);

        _ = response.EnsureSuccessStatusCode(); // Status Code 200-299
        BaseResponse<LoginResponse>? res = await response.Content.ReadFromJsonAsync<BaseResponse<LoginResponse>>();
        Assert.NotNull(res);
        Assert.Equal(401, res.Code);
    }
}