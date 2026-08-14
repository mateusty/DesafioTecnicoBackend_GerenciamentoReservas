using System.Net.Http.Headers;
using System.Net.Http.Json;

using Microsoft.AspNetCore.Mvc.Testing;

using Superpower.Parsers;

namespace IntegrationTests;

public class HotelIntegrationTests
{

    [Fact]
    public async Task WhenGettinHotels_ReturnsOk()
    {
        // Arrange
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        var loginDetails = new
        {
            Username = "teste@gmail.com",
            Password = "123"
        };

        await client.PostAsJsonAsync("/auth/register", loginDetails);

        var loginResponse = await client.PostAsJsonAsync(
            "/auth/login",
            loginDetails
            );

        var loginResult = await loginResponse.Content
        .ReadFromJsonAsync<TokenResponse>();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                loginResult!.Token
            );

        // Act
        var response = await client.GetAsync("/hotel");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(
            response.IsSuccessStatusCode,
            $"Status: {response.StatusCode}\nResponse: {content}"
            );
    }
}
