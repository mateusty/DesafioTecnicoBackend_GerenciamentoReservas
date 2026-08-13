using System.IdentityModel.Tokens.Jwt;

using Domain.Booking;
using Application.Booking;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Booking;

[ApiController]
[Route("[controller]")]
[Authorize]
public class BookingController : ControllerBase
{
    private readonly BookingService _bookingService;

    public BookingController(BookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetByUser()
    {
        Guid userId = Guid.Parse(User.FindFirst("sub").Value);

        var bookings = await _bookingService.GetByUser(userId);
        return Ok(bookings);
    }

    [HttpPost(Name = "PostBooking")]
    public async Task<IActionResult> PostBooking([FromBody] BookingRequest booking)
    {
        Guid userId = Guid.Parse(User.FindFirst("sub").Value);

        await _bookingService.PostBooking(booking, userId);
        return CreatedAtAction(nameof(GetByUser), new { userId = userId }, booking);
    }

    [HttpPut("{id}", Name = "EditBooking")]
    public async Task<IActionResult> EditBooking(int id, [FromBody] BookingRequest booking)
    {
        await _bookingService.EditBooking(booking, id);
        return NoContent();
    }

    [HttpDelete("{id}", Name = "DeleteBooking")]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        await _bookingService.DeleteBooking(id);
        return NoContent();
    }
}