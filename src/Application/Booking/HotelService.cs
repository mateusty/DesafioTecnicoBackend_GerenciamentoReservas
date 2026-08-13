using Application.Booking.Exceptions;

using Domain.Booking;

namespace Application.Booking;

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
            throw new NotFoundException("No hotels found");
        }
        return hotels;
    }

    public async Task<Hotel> GetHotelById(int id)
    {
        var hotel = await _hotelRepository.GetbyId(id);
        if (hotel == null)
        {
            throw new NotFoundException($"Hotel with ID {id} not found");
        }
        return hotel;
    }

    public async Task<int> PostHotel(HotelRequest hotelRequest)
    {
        Hotel hotel = new Hotel()
        {
            Name = hotelRequest.Name,
            Address = hotelRequest.Address,
            City = hotelRequest.City,
            Country = hotelRequest.Country,
            PricePerNight = hotelRequest.PricePerNight
        };

        return await _hotelRepository.Save(hotel);
    }

    public async Task DeleteHotel(int id)
    {
        var hotel = await _hotelRepository.GetbyId(id);

        if(hotel == null)
        {
            throw new NotFoundException($"Hotel with ID {id} not found");
        }

        await _hotelRepository.Delete(id);
    }

    public async Task EditHotel(HotelRequest hotelRequest, int id)
    {
        var hotelDb = await _hotelRepository.GetbyId(id);
        if (hotelDb == null)
        {
            throw new NotFoundException($"Hotel with ID {id} not found");
        }

        Hotel hotel = new Hotel()
        {
            Id = id,
            Name = hotelRequest.Name,
            Address = hotelRequest.Address,
            City = hotelRequest.City,
            Country = hotelRequest.Country,
            PricePerNight = hotelRequest.PricePerNight
        };

        await _hotelRepository.Edit(hotel);
    }
}