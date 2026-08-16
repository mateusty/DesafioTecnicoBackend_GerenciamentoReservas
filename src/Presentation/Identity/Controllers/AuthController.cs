using Domain.Identity;
using Application.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Identity;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly IHostEnvironment _environment;

    public AuthController(AuthService authService, IHostEnvironment environment)
    {
        _authService = authService;
        _environment = environment;
    }

    [HttpPost("login" , Name = "Login")]
    public async Task<IActionResult> DoLogin([FromBody] LoginRequest request)
    {
        var token = await _authService.DoLogin(request.Email, request.Password);
        var cookieOptions = new CookieOptions()
        {
            IsEssential = true,
            Expires = DateTime.UtcNow.AddMinutes(60),
            Secure = !_environment.IsEnvironment("Testing"),
            HttpOnly = true,
            SameSite = SameSiteMode.Lax
        };
        Response.Cookies.Append("accessToken", token, cookieOptions);
        return Ok(new { Token = token });
    }

    [HttpPost("logout", Name = "Logout")]
    public async Task<IActionResult> DoLogout()
    {
        var token = "";
        var cookieOptions = new CookieOptions()
        {
            IsEssential = true,
            Expires = DateTime.UtcNow.AddDays(-1),
            Secure = !_environment.IsEnvironment("Testing"),
            HttpOnly = true,
            SameSite = SameSiteMode.Lax
        };
        Response.Cookies.Append("accessToken", token, cookieOptions);
        return Ok();
    }

    [HttpPost("register", Name = "Register")]
    public async Task<ActionResult<string>> Register([FromBody] LoginRequest request)
    {
        await _authService.Register(request.Email, request.Password);

        return Ok("Registration successful");
    }
}