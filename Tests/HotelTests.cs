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
    public async Task GivenNonExistingHotelId_WhenGettingById_ThrowsNotFoundException()
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

    [Fact]
    public async Task GivenValidHotel_WhenPosting_IsSavedInDb()
    {
        // Arrange
        var hotel = new HotelRequest()
        {
            Name = "Teste",
            Address = "Teste",
            City = "Teste",
            Country = "Brazil",
            PricePerNight = 100
        };

        var mockRepo = new Mock<IHotelRepository>();

        var service = new HotelService(mockRepo.Object);

        // Act
        await service.PostHotel(hotel);

        // Assert
        mockRepo.Verify(r => r.Save(It.Is<Hotel>(h =>
            h.Name == hotel.Name &&
            h.Address == hotel.Address &&
            h.City == hotel.City &&
            h.Country == hotel.Country &&
            h.PricePerNight == hotel.PricePerNight
        )), Times.Once);
    }

    [Fact]
    public async Task GivenValidId_WhenDeleting_IsDeletedFromDb()
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
        mockRepo.Setup(r => r.GetbyId(hotel.Id)).ReturnsAsync(hotel);

        var service = new HotelService(mockRepo.Object);

        // Act
        await service.DeleteHotel(hotel.Id);

        // Assert
        mockRepo.Verify(r => r.Delete(hotel.Id), Times.Once);
    }

    [Fact]
    public async Task GivenNonExistingId_WhenDeleting_ThrowsNotFoundException()
    {
        // Arrange
        int id = 1;

        var mockRepo = new Mock<IHotelRepository>();
        mockRepo.Setup(r => r.GetbyId(It.IsAny<int>())).ReturnsAsync((Hotel?)null);

        var service = new HotelService(mockRepo.Object);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => service.DeleteHotel(id));

        // Assert
        mockRepo.Verify(r => r.GetbyId(id), Times.Once);
        Assert.Equal($"Hotel with ID {id} not found", exception.Message);
    }

    [Fact]
    public async Task GivenValidId_WhenEditing_IsEdited()
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

        var hotelRequest = new HotelRequest()
        {
            Name = "Teste1",
            Address = "Teste",
            City = "Teste",
            Country = "Brazil",
            PricePerNight = 100
        };

        var mockRepo = new Mock<IHotelRepository>();
        mockRepo.Setup(r => r.GetbyId(hotel.Id)).ReturnsAsync(hotel);

        var service = new HotelService(mockRepo.Object);

        // Act
        await service.EditHotel(hotelRequest, hotel.Id);

        // Assert
        mockRepo.Verify(r => r.GetbyId(hotel.Id), Times.Once);
        mockRepo.Verify(r => r.Edit(It.Is<Hotel>(h => 
            h.Id == hotel.Id &&
            h.Name == hotelRequest.Name &&
            h.Address == hotelRequest.Address &&
            h.City == hotelRequest.City &&
            h.Country == hotelRequest.Country &&
            h.PricePerNight == hotelRequest.PricePerNight
        )), Times.Once);
    }

    [Fact]
    public async Task GivenNonExistingId_WhenEditing_ThrowsNotFoundException()
    {
        // Arrange
        int id = 1;

        var hotelRequest = new HotelRequest()
        {
            Name = "Teste1",
            Address = "Teste",
            City = "Teste",
            Country = "Brazil",
            PricePerNight = 100
        };

        var mockRepo = new Mock<IHotelRepository>();
        mockRepo.Setup(r => r.GetbyId(id)).ReturnsAsync((Hotel?)null);

        var service = new HotelService(mockRepo.Object);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => service.EditHotel(hotelRequest, id));

        // Assert
        mockRepo.Verify(r => r.GetbyId(id), Times.Once);
        mockRepo.Verify(r => r.Edit(It.Is<Hotel>(h =>
            h.Id == id &&
            h.Name == hotelRequest.Name &&
            h.Address == hotelRequest.Address &&
            h.City == hotelRequest.City &&
            h.Country == hotelRequest.Country &&
            h.PricePerNight == hotelRequest.PricePerNight
        )), Times.Never);
        Assert.Equal($"Hotel with ID {id} not found", exception.Message);
    }
}
