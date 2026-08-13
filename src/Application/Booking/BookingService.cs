using Domain.Booking;
using Domain.Identity;

namespace Application.Booking;

public class BookingService
{

	private readonly IBookingRepository _bookingRepository;
    private readonly IHotelRepository _hotelRepository;

	public BookingService(IBookingRepository bookingRepository, IHotelRepository hotelRepository)
	{
		_bookingRepository = bookingRepository;
        _hotelRepository = hotelRepository;
	}

	public async Task<List<Bookings>> GetByUser(Guid userId)
    {
        return await _bookingRepository.GetbyUser(userId);
    }

	public async Task PostBooking(BookingRequest bookingRequest, Guid userId)
    {
		if (bookingRequest == null)
		{
            throw new ArgumentNullException(nameof(bookingRequest), "Booking cannot be null");
        }

        var hotelDb = await _hotelRepository.GetbyId(bookingRequest.HotelId);

        if (hotelDb == null)
        {
            throw new KeyNotFoundException($"Hotel with ID {bookingRequest.HotelId} not found");
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

        await _bookingRepository.Save(booking);
    }

	public async Task EditBooking(BookingRequest bookingRequest, int id)
    {
        var databaseBooking = _bookingRepository.GetbyId(id).Result;
        if (bookingRequest == null)
        {
            throw new ArgumentNullException(nameof(bookingRequest), "Booking cannot be null");
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
}