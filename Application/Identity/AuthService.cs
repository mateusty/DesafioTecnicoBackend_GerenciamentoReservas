using DesafioTecnicoBackend_GerenciamentoReservas.Domain.Identity;
using DesafioTecnicoBackend_GerenciamentoReservas.Infrastructure.Security;

namespace DesafioTecnicoBackend_GerenciamentoReservas.Application.Identity;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtService _jwtService;
    public AuthService(IUserRepository userRepository, JwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }
    public string DoLogin(string username, string password)
    {
        var user = _userRepository.GetByUsername(username).Result;
        if (user == null)
        {
            throw new UnauthorizedAccessException("Login failed: Invalid password or username");
        }
        bool isPasswordValid = PasswordHasher.VerifyPassword(password, user.PasswordHash);
        if (!isPasswordValid)
        {
            throw new UnauthorizedAccessException("Login failed: Invalid password or username");
        }
        var token = _jwtService.GenerateToken(user.Id, user.Username);
        return token;
    }
    public void Register(string username, string password)
    {
        _userRepository.Save(new User(Guid.CreateVersion7(), username, PasswordHasher.HashPassword(password)));
    }
}