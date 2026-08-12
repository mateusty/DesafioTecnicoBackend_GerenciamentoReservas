using DesafioTecnicoBackend_GerenciamentoReservas.Domain.Booking;

namespace DesafioTecnicoBackend_GerenciamentoReservas.Application.Booking;

public class HotelService
{
    private readonly IHotelRepository _hotelRepository;
    public HotelService(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public async Task<List<Hotel>> GetHotels()
    {
        var hotels = await _hotelRepository.GetAll();
        if (hotels == null)
        {
            throw new InvalidOperationException("No hotels found");
        }
        return hotels;
    }

    public async Task<Hotel> GetHotelById(int id)
    {
        var hotel = await _hotelRepository.GetbyId(id);
        if (hotel == null)
        {
            throw new KeyNotFoundException($"Hotel with ID {id} not found");
        }
        return hotel;
    }

    public async Task<int> PostHotel(HotelRequest hotel)
    {
        if (hotel == null)
        {
            throw new ArgumentNullException(nameof(hotel), "Hotel cannot be null");
        }
        return await _hotelRepository.Save(hotel);
    }

    public async Task DeleteHotel(int id)
    {
        await _hotelRepository.Delete(id);
    }

    public async Task EditHotel(HotelRequest hotelRequest, int id)
    {
        var hotel = _hotelRepository.GetbyId(id);
        if (hotel == null)
        {
            throw new KeyNotFoundException($"Hotel with ID {id} not found");
        }
        _hotelRepository.Edit(hotelRequest, id);
    }
}