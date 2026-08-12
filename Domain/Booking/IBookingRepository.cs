namespace DesafioTecnicoBackend_GerenciamentoReservas.Domain.Booking;

public interface IBookingRepository
{
    Task<Booking?> GetByUserAndHotel(int userId, int hotelId);
    Task<List<Booking>> GetByUser(int userId);
    Task Save(Booking booking);
}