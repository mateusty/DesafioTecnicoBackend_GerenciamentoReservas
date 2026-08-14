using Application.Booking;
using Application.Booking.Exceptions;

using Domain.Booking;

using Moq;

namespace UnitTests;

public class BookingUnitTests
{
    [Fact]
    public async Task GivenEnoughDataInDb_WhenGettingByUser_ReturnsAllBookings()
    {
        // Arrange
        Guid userId = Guid.CreateVersion7();

        List<Bookings> bookings = Enumerable.Range(1, 5)
        .Select(id => new Bookings
        {
            Id = id,
            UserId = userId,
            HotelId = 1,
            RoomNumber = 100,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(3)
        })
        .ToList();

        var mockRepoHotel = new Mock<IHotelRepository>();

        var mockRepoBooking = new Mock<IBookingRepository>();
        mockRepoBooking.Setup(r => r.GetbyUser(userId)).ReturnsAsync(bookings);

        var service = new BookingService(mockRepoBooking.Object, mockRepoHotel.Object);

        // Act
        var result = await service.GetByUser(userId);

        // Assert
        Assert.Equal(bookings, result);

        mockRepoBooking.Verify(r => r.GetbyUser(userId), Times.Once);
    }

    [Fact]
    public async Task GivenValidHotelId_WhenPosting_SaveBooking()
    {
        // Arrange
        Guid userId = Guid.CreateVersion7();

        BookingRequest booking = new BookingRequest()
        {
            HotelId = 1,
            RoomNumber = 100,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(3),
            Status = "Pending"
        };

        var mockRepoHotel = new Mock<IHotelRepository>();
        mockRepoHotel.Setup(r => r.GetbyId(booking.HotelId)).ReturnsAsync(new Hotel());

        var mockRepoBooking = new Mock<IBookingRepository>();

        var service = new BookingService(mockRepoBooking.Object, mockRepoHotel.Object);

        // Act
        await service.PostBooking(booking, userId);

        // Assert
        mockRepoBooking.Verify(r => r.Save(It.Is<Bookings>(b =>
            b.UserId == userId &&
            b.HotelId == booking.HotelId &&
            b.RoomNumber == booking.RoomNumber &&
            b.StartDate == booking.StartDate &&
            b.EndDate == booking.EndDate &&
            b.Status == booking.Status
        )), Times.Once);
        mockRepoHotel.Verify(r => r.GetbyId(booking.HotelId), Times.Once);
    }

    [Fact]
    public async Task GivenNonExistingHotelId_WhenPosting_ThrowsNotFoundException()
    {
        // Arrange
        Guid userId = Guid.CreateVersion7();

        BookingRequest booking = new BookingRequest()
        {
            HotelId = 999,
            RoomNumber = 100,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(3),
            Status = "Pending"
        };

        var mockRepoHotel = new Mock<IHotelRepository>();
        mockRepoHotel.Setup(r => r.GetbyId(booking.HotelId)).ReturnsAsync((Hotel?)null);

        var mockRepoBooking = new Mock<IBookingRepository>();

        var service = new BookingService(mockRepoBooking.Object, mockRepoHotel.Object);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => service.PostBooking(booking, userId));

        // Assert
        Assert.Equal($"Hotel with ID {booking.HotelId} not found", exception.Message);
        mockRepoHotel.Verify(r => r.GetbyId(booking.HotelId), Times.Once);
    }

    [Fact]
    public async Task GivenValidId_WhenEditing_UpdatesBooking()
    {
        // Arrange
        Guid userId = Guid.CreateVersion7();
        int bookingId = 1;

        BookingRequest booking = new BookingRequest()
        {
            HotelId = 1,
            RoomNumber = 100,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(3),
            Status = "Pending"
        };

        Bookings databaseBooking = new Bookings()
        {
            Id = bookingId,
            UserId = userId,
            HotelId = booking.HotelId
        };

        var mockRepoHotel = new Mock<IHotelRepository>();

        var mockRepoBooking = new Mock<IBookingRepository>();
        mockRepoBooking.Setup(r => r.GetbyId(bookingId)).ReturnsAsync(databaseBooking);

        var service = new BookingService(mockRepoBooking.Object, mockRepoHotel.Object);

        // Act
        await service.EditBooking(booking, bookingId);

        // Assert
        mockRepoBooking.Verify(r => r.GetbyId(bookingId), Times.Once);
        mockRepoBooking.Verify(r => r.Edit(It.Is<Bookings>(b =>
            b.Id == bookingId &&
            b.UserId == userId &&
            b.HotelId == booking.HotelId &&
            b.RoomNumber == booking.RoomNumber &&
            b.StartDate == booking.StartDate &&
            b.EndDate == booking.EndDate &&
            b.Status == booking.Status
        )), Times.Once);
    }

    [Fact]
    public async Task GivenNonExistingId_WhenEditing_ThrowNotFoundException()
    {
        // Arrange
        int bookingId = 1;

        BookingRequest booking = new BookingRequest()
        {
            HotelId = 1,
            RoomNumber = 100,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(3),
            Status = "Pending"
        };

        var mockRepoHotel = new Mock<IHotelRepository>();

        var mockRepoBooking = new Mock<IBookingRepository>();
        mockRepoBooking.Setup(r => r.GetbyId(bookingId)).ReturnsAsync((Bookings?)null);

        var service = new BookingService(mockRepoBooking.Object, mockRepoHotel.Object);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => service.EditBooking(booking, bookingId));

        // Assert
        mockRepoBooking.Verify(r => r.GetbyId(bookingId), Times.Once);
        Assert.Equal($"Booking with ID {bookingId} not found", exception.Message);
    }

    [Fact]
    public async Task GivenValidId_WhenDeleting_DeleteBooking()
    {
        // Arrange
        int bookingId = 1;

        var mockRepoHotel = new Mock<IHotelRepository>();

        var mockRepoBooking = new Mock<IBookingRepository>();
        mockRepoBooking.Setup(r => r.GetbyId(bookingId)).ReturnsAsync(new Bookings());

        var service = new BookingService(mockRepoBooking.Object, mockRepoHotel.Object);

        // Act
        await service.DeleteBooking(bookingId);

        // Assert
        mockRepoBooking.Verify(r => r.GetbyId(bookingId), Times.Once);
        mockRepoBooking.Verify(r => r.Delete(bookingId), Times.Once);
    }

    [Fact]
    public async Task GivenNonExistingId_WhenDeleting_ThrowNotFoundException()
    {
        // Arrange
        int bookingId = 999;

        var mockRepoHotel = new Mock<IHotelRepository>();

        var mockRepoBooking = new Mock<IBookingRepository>();
        mockRepoBooking.Setup(r => r.GetbyId(It.IsAny<int>())).ReturnsAsync((Bookings?)null);

        var service = new BookingService(mockRepoBooking.Object, mockRepoHotel.Object);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => service.DeleteBooking(bookingId));

        // Assert
        mockRepoBooking.Verify(r => r.GetbyId(bookingId), Times.Once);
    }
}

