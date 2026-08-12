using Microsoft.AspNetCore.Authorization;

using DesafioTecnicoBackend_GerenciamentoReservas.Domain.Booking;
using DesafioTecnicoBackend_GerenciamentoReservas.Application.Booking;

using Microsoft.AspNetCore.Mvc;

namespace DesafioTecnicoBackend_GerenciamentoReservas.Presentation.Booking;

[ApiController]
[Route("[controller]")]
[Authorize]
public class HotelController : ControllerBase
{
    private readonly HotelService _hotelService;

    public HotelController(HotelService hotelService)
    {
        _hotelService = hotelService;
    }

    [HttpGet(Name = "GetHotels")]
    public async Task<IActionResult> GetHotels()
    {
        var hotels = await _hotelService.GetHotels();
        return Ok(hotels);
    }

    [HttpGet("{id}", Name = "GetHotelById")]
    public async Task<IActionResult> GetHotelById(int id)
    {
        var hotel = await _hotelService.GetHotelById(id);
        return Ok(hotel);
    }

    [HttpPost(Name = "PostHotel")]
    public async Task<IActionResult> PostHotel([FromBody] HotelRequest hotel)
    {
        var id = await _hotelService.PostHotel(hotel);
        return CreatedAtAction(nameof(PostHotel), new { id }, hotel);
    }

    [HttpDelete("{id}", Name = "DeleteHotel")]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        await _hotelService.DeleteHotel(id);
        return NoContent();
    }

    [HttpPut("{id}", Name = "PutHotel")]
    public async Task<IActionResult> EditHotel([FromBody] HotelRequest hotel, int id)
    {
        await _hotelService.EditHotel(hotel, id);
        return NoContent();
    }
}