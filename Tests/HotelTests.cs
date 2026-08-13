using Application.Booking;

using Moq;
using Domain.Booking;

namespace Tests;

public class HotelTests
{
    [Fact]
    public async Task DadoDeHotelValido_QuandoBuscarPorId_RetornarDadoDoHotel()
    {
        // Arrange
        var hotel = new Hotel()
        {
            Id = 1,
            Name = "Teste",
            Address = "Teste",
            City = "Teste",
            Country = "Brazil",
            PricePerNight = 100
        };

        var mockRepo = new Mock<IHotelRepository>();
        mockRepo.Setup(r => r.GetbyId(1)).ReturnsAsync(hotel);

        var service = new HotelService(mockRepo.Object);

        // Act
        var result = await service.GetHotelById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(hotel.Id, result.Id);
        Assert.Equal(hotel.Name, result.Name);

        mockRepo.Verify(r => r.GetbyId(1), Times.Once);
    }
}
