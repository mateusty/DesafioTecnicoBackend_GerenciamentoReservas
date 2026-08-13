using Domain.Identity;
using Application.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Identity;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login" , Name = "Login")]
    public IActionResult DoLogin([FromBody] LoginRequest request)
    {
        var token = _authService.DoLogin(request.Username, request.Password);
        return Ok(new { Token = token });
    }

    [HttpPost("register", Name = "Register")]
    public ActionResult<string> Register([FromBody] LoginRequest request)
    {
        _authService.Register(request.Username, request.Password);

        return Ok("Registration successful");
    }
}