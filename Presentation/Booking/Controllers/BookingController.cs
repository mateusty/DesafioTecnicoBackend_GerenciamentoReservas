using DesafioTecnicoBackend_GerenciamentoReservas.Domain.Booking;
using DesafioTecnicoBackend_GerenciamentoReservas.Application.Booking;

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