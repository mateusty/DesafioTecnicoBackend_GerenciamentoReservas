namespace DesafioTecnicoBackend_GerenciamentoReservas.Application.Booking;

public class HotelRequest
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public decimal PricePerNight { get; set; }
}