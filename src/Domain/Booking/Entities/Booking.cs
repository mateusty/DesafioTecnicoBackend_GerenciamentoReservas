namespace Domain.Booking;

public class Bookings
{
	public int Id { get; set; }
	public Guid UserId { get; set; }
	public int HotelId { get; set; }
	public int RoomNumber { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public string Status { get; set; } = "Pending";
}