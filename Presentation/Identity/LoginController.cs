using Microsoft.AspNetCore.Mvc;

namespace DesafioTecnicoBackend_GerenciamentoReservas.Presentation.Identity;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    [HttpPost(Name = "DoLogin")]
    public ActionResult<string> DoLogin()
    {
        return Ok("Login successful");
    }
}