using DesafioTecnicoBackend_GerenciamentoReservas.Application.Booking;

namespace DesafioTecnicoBackend_GerenciamentoReservas.Domain.Booking;

public class Hotel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public decimal PricePerNight { get; set; }

    public Hotel(HotelRequest hotelRequest, int id)
    {
        Id = id;
        Name = hotelRequest.Name;
        Address = hotelRequest.Address;
        City = hotelRequest.City;
        Country = hotelRequest.Country;
        PricePerNight = hotelRequest.PricePerNight;
    }

    public Hotel()
    {
    }
}