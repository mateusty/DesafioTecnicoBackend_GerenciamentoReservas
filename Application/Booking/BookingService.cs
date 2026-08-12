using DesafioTecnicoBackend_GerenciamentoReservas.Domain.Booking;
using DesafioTecnicoBackend_GerenciamentoReservas.Domain.Booking;

namespace DesafioTecnicoBackend_GerenciamentoReservas.Application.Booking;

public class BookingService
{

	private readonly IBookingRepository _bookingRepository;

	public BookingService(IBookingRepository bookingRepository)
	{
		_bookingRepository = bookingRepository;
	}

	public List<Bookings> GetByUser(Guid userId)
    {
        return _bookingRepository.GetbyUser(userId).Result;
    }

	public void PostBooking(BookingRequest booking, Guid userId)
    {
		if (booking == null)
		{
            throw new ArgumentNullException(nameof(booking), "Booking cannot be null");
        }
        _bookingRepository.Save(booking, userId);
    }

	public void EditBooking(BookingRequest booking, int id)
    {
        var databaseBooking = _bookingRepository.GetbyId(id).Result;
        if (booking == null)
        {
            throw new ArgumentNullException(nameof(booking), "Booking cannot be null");
        }
        _bookingRepository.Edit(booking, id, databaseBooking.UserId);
    }
}