namespace Domain.Booking;

public interface IBookingRepository
{
    Task<Bookings> GetbyId(int id);
    Task<List<Bookings>> GetbyUser(Guid userId);
    Task Save(Bookings booking);
    Task Edit(Bookings booking);
    Task Delete(int id);
}