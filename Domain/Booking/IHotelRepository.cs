namespace DesafioTecnicoBackend_GerenciamentoReservas.Domain.Booking;

public interface IHotelRepository
{
    Task<Hotel?> GetbyId(int id);
    Task<List<Hotel>> GetAll();
    Task Save(Hotel hotel);
}