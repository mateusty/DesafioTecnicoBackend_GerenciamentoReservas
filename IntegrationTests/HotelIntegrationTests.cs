using System.Net.Http.Headers;
using System.Net.Http.Json;

using Application.Booking;

using Domain.Booking;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
namespace IntegrationTests;

public class HotelIntegrationTests : IClassFixture<TestWebApplicationFactory>, IAsyncLifetime
{
    private readonly TestWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public HotelIntegrationTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        await AuthenticateAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }


    // Método auxiliar que registra e loga um usuário para testes
    private async Task AuthenticateAsync()
    {
        var loginDetails = new
        {
            Email = "teste@gmail.com",
            Password = "123"
        };

        await _client.PostAsJsonAsync("/auth/register", loginDetails);

        var loginResponse = await _client.PostAsJsonAsync(
            "/auth/login",
            loginDetails
        );
    }

    [Fact]
    public async Task GivenHotelsInDatabase_WhenGettingHotels_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/hotel");

        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task GivenValidHotel_WhenPostingHotel_ReturnsCreated()
    {
        // Arrange
        HotelRequest hotel = new HotelRequest()
        {
            Name = "TesteIntegração",
            Address = "TesteIntegração",
            City = "TesteIntegração",
            Country = "TesteIntegração",
            PricePerNight = 100
        };

        // Act
        var response = await _client.PostAsJsonAsync("/hotel", hotel);

        // Assert
        Assert.Equal(
            ((int)StatusCodes.Status201Created),
            ((int)response.StatusCode)
            );
    }

    [Fact]
    public async Task GivenHotelId_WhenEditing_ReturnsNoContentOrNotFound()
    {
        // Arrange
        await CreateHotelAsync();

        var getResponse = await _client.GetAsync("/hotel");
        var content = await getResponse.Content.ReadFromJsonAsync<List<Hotel>>();

        // Verifica se há hotéis no banco de dados, se não houver, o teste falha
        Assert.True(getResponse.IsSuccessStatusCode);

        int hotelId = content![0].Id;

        HotelRequest hotel = new HotelRequest()
        {
            Name = "TesteIntegração",
            Address = "TesteIntegração",
            City = "TesteIntegração",
            Country = "TesteIntegração",
            PricePerNight = 100
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/hotel/{hotelId}", hotel);

        // Assert
        Assert.True(((int)response.StatusCode) == 204);
    }

    [Fact]
    public async Task GivenHotelId_WhenDeleting_ReturnsNoContentOrNotFound()
    {
        // Arrange
        await CreateHotelAsync();

        var getResponse = await _client.GetAsync("/hotel");
        var content = await getResponse.Content.ReadFromJsonAsync<List<Hotel>>();

        // Verifica se há hotéis no banco de dados, se não houver, o teste falha
        Assert.True(getResponse.IsSuccessStatusCode);

        int hotelId = content![0].Id;

        // Act
        var response = await _client.DeleteAsync($"/hotel/{hotelId}");
        var body = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"Body: {body}");

        // Assert
        Assert.True(((int)response.StatusCode) == 204);
    }

    // Método auxiliar para criar hotel
    private async Task CreateHotelAsync()
    {
        var request = new
        {
            Name = "TesteIntegração",
            Address = "TesteIntegração",
            City = "TesteIntegração",
            Country = "TesteIntegração",
            PricePerNight = 100
        };

        var response = await _client.PostAsJsonAsync("/hotel", request);

        response.EnsureSuccessStatusCode();
    }
}
