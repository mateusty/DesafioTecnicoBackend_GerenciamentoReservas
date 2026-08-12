using DesafioTecnicoBackend_GerenciamentoReservas.Application.Booking;

namespace DesafioTecnicoBackend_GerenciamentoReservas.Domain.Booking;

public class Bookings
{
	public int Id { get; set; }
	public Guid UserId { get; set; }
	public int HotelId { get; set; }
	public int RoomNumber { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public string Status { get; set; } = "Pending";

	public Bookings(BookingRequest booking, int id, Guid userId)
    {
        Id = id;
        UserId = userId;
        HotelId = booking.HotelId;
        RoomNumber = booking.RoomNumber;
        StartDate = booking.StartDate;
        EndDate = booking.EndDate;
        Status = booking.Status;
    }

    public Bookings(int id, Guid userId, int hotelId, int roomNumber, DateTime startDate, DateTime endDate, string status)
    {
        Id = id;
        UserId = userId;
        HotelId = hotelId;
        RoomNumber = roomNumber;
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
    }
}