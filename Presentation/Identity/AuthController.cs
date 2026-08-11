using DesafioTecnicoBackend_GerenciamentoReservas.Domain.Identity;
using DesafioTecnicoBackend_GerenciamentoReservas.Infrastructure.Identity;
using DesafioTecnicoBackend_GerenciamentoReservas.Infrastructure.Security;
using Microsoft.AspNetCore.Mvc;

namespace DesafioTecnicoBackend_GerenciamentoReservas.Presentation.Identity;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly JwtService _jwtService;

    public AuthController(IUserRepository userRepository, JwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    [HttpPost("login" , Name = "Login")]
    public IActionResult DoLogin([FromBody] LoginRequest request)
    {
        var user = _userRepository.GetByUsername(request.Username).Result;
        if (user == null)
        {
            return Unauthorized("Login failed: Invalid password or username");
        }

        bool isPasswordValid = PasswordHasher.VerifyPassword(request.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            return Unauthorized("Login failed: Invalid password or username");
        }
        var token = _jwtService.GenerateToken(user.Id, user.Username);
        return Ok(new { Token = token });
    }

    [HttpPost("register", Name = "Register")]
    public ActionResult<string> Register([FromBody] LoginRequest request)
    {
        _userRepository.Save(new User(Guid.CreateVersion7(), request.Username, PasswordHasher.HashPassword(request.Password)));

        return Ok("Registration successful");
    }
}