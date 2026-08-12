namespace DesafioTecnicoBackend_GerenciamentoReservas.Domain.Booking;

public class Booking
{
	public int Id { get; set; }
	public Guid UserId { get; set; }
	public int RoomId { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public string Status { get; set; } = "Pending";
}