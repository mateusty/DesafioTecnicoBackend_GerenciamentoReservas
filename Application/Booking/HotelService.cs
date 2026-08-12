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
        var hotels = _hotelRepository.GetAll();
        if(hotels == null) {
            throw new InvalidOperationException("No hotels found");
        }
        return hotels;
    }

    public void PostHotel(Hotel hotel)
    {
        if (hotel == null)
        {
            throw new ArgumentNullException(nameof(hotel), "Hotel cannot be null");
        }
        _hotelRepository.Save(hotel);
}