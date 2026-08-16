using Application.Booking.Exceptions;

using Domain.Booking;
using Domain.Identity;

using Infrastructure.RabbitMQ;

using MassTransit;

namespace Application.Booking;

public class BookingService
{

	private readonly IBookingRepository _bookingRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPublishEndpoint _publishEndpoint;

	public BookingService(IBookingRepository bookingRepository, IHotelRepository hotelRepository, IUserRepository userRepository, IPublishEndpoint publishEndpoint)
	{
		_bookingRepository = bookingRepository;
        _hotelRepository = hotelRepository;
        _userRepository = userRepository;
        _publishEndpoint = publishEndpoint;

    }

	public async Task<List<Bookings>> GetByUser(Guid userId)
    {
        return await _bookingRepository.GetbyUser(userId);
    }

	public async Task PostBooking(BookingRequest bookingRequest, Guid userId)
    {
        var hotelDb = await _hotelRepository.GetbyId(bookingRequest.HotelId);

        if (hotelDb == null)
        {
            throw new NotFoundException($"Hotel with ID {bookingRequest.HotelId} not found");
        }

        Bookings booking = new Bookings()
        {
            UserId = userId,
            HotelId = bookingRequest.HotelId,
            RoomNumber = bookingRequest.RoomNumber,
            StartDate = bookingRequest.StartDate,
            EndDate = bookingRequest.EndDate,
            Status = bookingRequest.Status
        };

        // Pegando informações extras para mandar o evento para o RabbitMQ
        var user = await _userRepository.GetbyId(userId);
        var userEmail = user!.Email;

        // Manda o evento para a fila do RabbitMQ
        await _publishEndpoint.Publish<NewBookingEmail>(new
        {
            ReceiverEmail = userEmail,
            HotelName = hotelDb.Name,
            HotelAddress = hotelDb.Address,
            bookingRequest.StartDate,
            bookingRequest.EndDate
        });

        await _bookingRepository.Save(booking);
    }

	public async Task EditBooking(BookingRequest bookingRequest, int id)
    {
        var databaseBooking = await _bookingRepository.GetbyId(id);

        if (databaseBooking == null)
        {
            throw new NotFoundException($"Booking with ID {id} not found");
        }

        Bookings booking = new Bookings()
        {
            Id = id,
            UserId = databaseBooking.UserId,
            HotelId = bookingRequest.HotelId,
            RoomNumber = bookingRequest.RoomNumber,
            StartDate = bookingRequest.StartDate,
            EndDate = bookingRequest.EndDate,
            Status = bookingRequest.Status
        };

        await _bookingRepository.Edit(booking);
    }

    public async Task DeleteBooking(int id)
    {
        var booking = await _bookingRepository.GetbyId(id);

        if (booking == null)
        {
            throw new NotFoundException($"Booking with ID {id} not found");
        }

        await _bookingRepository.Delete(id);
    }
}