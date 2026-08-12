using DesafioTecnicoBackend_GerenciamentoReservas.Domain.Identity;
using DesafioTecnicoBackend_GerenciamentoReservas.Application.Identity;

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

    [HttpGet(Name = "GetHotels"))]
    public IActionResult GetHotels()
    {
        var hotels = _hotelService.GetHotels();
        return Ok(hotels);
    }

    [HttpPost(Name = "PostHotel")]
    public IActionResult PostHotel([FromBody] Hotel hotel)
    {
        _hotelService.PostHotel(hotel);
        return CreatedAtAction(nameof(PostHotel), new { id = hotel.Id }, hotel);
    }
}