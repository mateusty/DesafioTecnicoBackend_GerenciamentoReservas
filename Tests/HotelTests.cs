using Application.Booking;

using Moq;
using Domain.Booking;
using Application.Booking.Exceptions;

namespace Tests;

public class HotelTests
{
    [Fact]
    public async Task GivenEnoughDataInDb_WhenGettingAll_ReturnsAllHotelData()
    {
        // Arrange
        List<Hotel> hotels = Enumerable.Range(1, 5)
        .Select(id => new Hotel
        {
            Id = id,
            Name = "Teste",
            Address = "Teste",
            City = "Teste",
            Country = "Brazil",
            PricePerNight = 100
        })
        .ToList();

        var mockRepo = new Mock<IHotelRepository>();
        mockRepo.Setup(r => r.GetAll()).ReturnsAsync(hotels);

        var service = new HotelService(mockRepo.Object);

        // Act
        var result = await service.GetHotels();

        // Assert
        Assert.Equal(hotels, result);

        mockRepo.Verify(r => r.GetAll(), Times.Once);
    }

    [Fact]
    public async Task GivenNoneHotelsInDb_WhenGettingAll_ThrowsNotFoundException()
    {
        // Arrange
        var mockRepo = new Mock<IHotelRepository>();
        mockRepo.Setup(r => r.GetAll()).ReturnsAsync((List<Hotel>?)null);

        var service = new HotelService(mockRepo.Object);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => service.GetHotels());

        // Assert
        Assert.Equal("No hotels found", exception.Message);
        mockRepo.Verify(r => r.GetAll(), Times.Once);
    }

    [Fact]
    public async Task GivenValidHotelId_WhenGettingById_ReturnsHotelData()
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

    [Fact]
    public async Task GivenNotExistingHotelId_WhenGettingById_ThrowsNotFoundException()
    {
        // Arrange
        int id = 1;

        var mockRepo = new Mock<IHotelRepository>();
        mockRepo.Setup(r => r.GetbyId(It.IsAny<int>())).ReturnsAsync((Hotel?)null);

        var service = new HotelService(mockRepo.Object);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => service.GetHotelById(id));

        // Assert
        mockRepo.Verify(r => r.GetbyId(id), Times.Once);
        Assert.Equal($"Hotel with ID {id} not found", exception.Message);
    }
}
