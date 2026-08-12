using DesafioTecnicoBackend_GerenciamentoReservas.Domain.Booking;
using DesafioTecnicoBackend_GerenciamentoReservas.Application.Booking;

using Microsoft.AspNetCore.Mvc;

namespace DesafioTecnicoBackend_GerenciamentoReservas.Presentation.Booking;

[ApiController]
[Route("[controller]")]
public class HotelController : ControllerBase
{
    private readonly HotelService _hotelService;

    public HotelController(HotelService hotelService)
    {
        _hotelService = hotelService;
    }

    [HttpGet(Name = "GetHotels")]
    public IActionResult GetHotels()
    {
        var hotels = _hotelService.GetHotels();
        return Ok(hotels);
    }

    [HttpGet("{id}", Name = "GetHotelById")]
    public IActionResult GetHotelById(int id)
    {
        var hotel = _hotelService.GetHotelById(id);
        return Ok(hotel);
    }

    [HttpPost(Name = "PostHotel")]
    public IActionResult PostHotel([FromBody] HotelRequest hotel)
    {
        _hotelService.PostHotel(hotel);
        return CreatedAtAction(nameof(PostHotel), hotel);
    }
}