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
    public IActionResult GetByUser()
    {
        Guid userId = Guid.Parse(User.FindFirst("sub").Value);

        var bookings = _bookingService.GetByUser(userId);
        return Ok(bookings);
    }

    [HttpPost(Name = "PostBooking")]
    public IActionResult PostBooking([FromBody] BookingRequest booking)
    {
        Guid userId = Guid.Parse(User.FindFirst("sub").Value);

        _bookingService.PostBooking(booking, userId);
        return CreatedAtAction(nameof(GetByUser), new { userId = userId }, booking);
    }

    [HttpPut("{id}", Name = "EditBooking")]
    public IActionResult EditBooking(int id, [FromBody] BookingRequest booking)
    {
        _bookingService.EditBooking(booking, id);
        return NoContent();
    }
}