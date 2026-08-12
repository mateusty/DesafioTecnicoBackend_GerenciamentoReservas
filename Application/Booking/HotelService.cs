using DesafioTecnicoBackend_GerenciamentoReservas.Domain.Booking;

namespace DesafioTecnicoBackend_GerenciamentoReservas.Application.Booking;

public class HotelService
{
    private readonly IHotelRepository _hotelRepository;
    public HotelService(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public List<Hotel> GetHotels()
    {
        var hotels = _hotelRepository.GetAll().Result;
        if (hotels == null)
        {
            throw new InvalidOperationException("No hotels found");
        }
        return hotels;
    }

    public Hotel GetHotelById(int id)
    {
        var hotel = _hotelRepository.GetbyId(id).Result;
        if (hotel == null)
        {
            throw new KeyNotFoundException($"Hotel with ID {id} not found");
        }
        return hotel;
    }

    public int PostHotel(HotelRequest hotel)
    {
        if (hotel == null)
        {
            throw new ArgumentNullException(nameof(hotel), "Hotel cannot be null");
        }
        return _hotelRepository.Save(hotel).Result;
    }
}