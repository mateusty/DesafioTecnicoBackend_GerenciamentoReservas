namespace DesafioTecnicoBackend_GerenciamentoReservas.Application.Booking;

public class BookingService
{
	private readonly IBookingRepository _bookingRepository;
	public BookingService(IBookingRepository bookingRepository)
	{
		_bookingRepository = bookingRepository;
	}
}