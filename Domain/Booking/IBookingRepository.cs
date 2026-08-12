using DesafioTecnicoBackend_GerenciamentoReservas.Application.Booking;

namespace DesafioTecnicoBackend_GerenciamentoReservas.Domain.Booking;

public interface IBookingRepository
{
    Task<Bookings> GetbyId(int id);
    Task<List<Bookings>> GetbyUser(Guid userId);
    Task Save(BookingRequest booking, Guid userId);
    Task Edit(BookingRequest booking, int id, Guid userId);
}