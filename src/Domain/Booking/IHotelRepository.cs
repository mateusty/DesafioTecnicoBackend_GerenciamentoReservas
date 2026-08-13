namespace Domain.Booking;

public interface IHotelRepository
{
    Task<Hotel?> GetbyId(int id);
    Task<List<Hotel>> GetAll();
    Task<int> Save(Hotel hotel);
    Task Edit(Hotel hotel);
    Task Delete(int id);
}