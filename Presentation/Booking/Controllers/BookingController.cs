using DesafioTecnicoBackend_GerenciamentoReservas.Domain.Identity;
using DesafioTecnicoBackend_GerenciamentoReservas.Application.Identity;

using Microsoft.AspNetCore.Mvc;

namespace DesafioTecnicoBackend_GerenciamentoReservas.Presentation.Booking;

[ApiController]
[Route("[controller]")]
public class BookingController : ControllerBase
{
    private readonly BookingService _bookingService;

    public BookingController(BookingService bookingService)
    {
        _bookingService = bookingService;
    }


}