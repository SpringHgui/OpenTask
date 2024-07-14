// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using OpenTask.Application.Base;
using OpenTask.Application.Dashboard;

namespace OpenTask.WebApi.Tests.IntegrationTests
{
    public class DashboardTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient client;

        public DashboardTest(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            client = _factory.CreateClient();
        }

        [Theory]
        [InlineData("/api/Dashboard/Statistics")]
        public async Task Get_Statistics(string url)
        {
            HttpResponseMessage response = await client.GetAsync(url);

            _ = response.EnsureSuccessStatusCode();
            var res = await response.Content.ReadFromJsonAsync<BaseResponse<StatisticsResponse>>();

            Assert.NotNull(res);
            Assert.Equal(401, res.Code);
        }
    }
}
