namespace Application.Booking;

public class BookingRequest
{
    public int HotelId { get; set; }
    public int RoomNumber { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = "Pending";
}