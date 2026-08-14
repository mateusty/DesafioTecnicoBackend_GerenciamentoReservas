using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

using Application.Booking;

using Domain.Booking;

using Microsoft.AspNetCore.Http;

namespace IntegrationTests;
public class BookingIntegrationTests : IClassFixture<TestWebApplicationFactory>, IAsyncLifetime
{
    private readonly TestWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public BookingIntegrationTests(TestWebApplicationFactory factory)
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
            Username = "teste@gmail.com",
            Password = "123"
        };

        await _client.PostAsJsonAsync("/auth/register", loginDetails);

        var loginResponse = await _client.PostAsJsonAsync(
            "/auth/login",
            loginDetails
        );
    }

    [Fact]
    public async Task GivenBookingsInDatabase_WhenGettingBookings_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/booking");

        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task GivenValidBooking_WhenPostingBooking_ReturnsCreated()
    {
        // Arrange
        await CreateHotelAsync();

        var getResponse = await _client.GetAsync("/hotel");
        var content = await getResponse.Content.ReadFromJsonAsync<List<Hotel>>();

        Assert.True(getResponse.IsSuccessStatusCode);

        int hotelId = content![0].Id;

        BookingRequest booking = new BookingRequest()
        {
            HotelId = hotelId,
            RoomNumber = 201,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(3),
            Status = "Pending"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/booking", booking);

        // Assert
        Assert.Equal(
            ((int)StatusCodes.Status201Created),
            ((int)response.StatusCode)
            ); 
    }

    [Fact]
    public async Task GivenBookingId_WhenEditing_ReturnsNoContentOrNotFound()
    {
        // Arrange
        // Pegando ID de reserva válida
        await CreateBookingAsync();

        var getResponse = await _client.GetAsync("/booking");
        var content = await getResponse.Content.ReadFromJsonAsync<List<Bookings>>();

        Assert.True(getResponse.IsSuccessStatusCode);

        // Pegando ID de hotel válido
        await CreateHotelAsync();

        var getResponseHotel = await _client.GetAsync("/hotel");
        var contentHotel = await getResponseHotel.Content.ReadFromJsonAsync<List<Hotel>>();

        // Verifica se há hotéis no banco de dados, se não houver, o teste falha
        Assert.True(getResponseHotel.IsSuccessStatusCode);

        int hotelId = contentHotel![0].Id;

        int bookingId = content![0].Id;

        BookingRequest booking = new BookingRequest()
        {
            HotelId = hotelId,
            RoomNumber = 211,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(5),
            Status = "Pending"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/booking/{bookingId}", booking);

        // Assert
        Assert.True(((int)response.StatusCode) == 204 || ((int)response.StatusCode) == 404);
    }

    [Fact]
    public async Task GivenBookingId_WhenDeleting_ReturnsNoContentOrNotFound()
    {
        // Arrange
        // Pegando ID de reserva válida
        await CreateBookingAsync();

        var getResponse = await _client.GetAsync("/booking");
        var content = await getResponse.Content.ReadFromJsonAsync<List<Bookings>>();

        Assert.True(getResponse.IsSuccessStatusCode);

        int bookingId = content![0].Id;

        // Act
        var response = await _client.DeleteAsync($"/booking/{bookingId}");

        // Assert
        Assert.True(((int)response.StatusCode) == 204 || ((int)response.StatusCode) == 404);
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

    // Método auxiliar para criar reserva
    private async Task CreateBookingAsync()
    {
        await CreateHotelAsync();

        var response = await _client.GetAsync("/hotel");
        var content = await response.Content.ReadFromJsonAsync<List<Hotel>>();

        int hotelId = content![0].Id;

        var request = new
        {
            HotelId = hotelId,
            RoomNumber = 202,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(3),
            Status = "Pending"
        };

        var postResponse = await _client.PostAsJsonAsync("/booking", request);

        postResponse.EnsureSuccessStatusCode();
    }
}

