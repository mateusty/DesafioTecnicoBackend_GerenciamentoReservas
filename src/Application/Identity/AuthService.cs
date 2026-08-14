using Domain.Identity;

namespace Application.Identity;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    public AuthService(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }
    public async Task<string> DoLogin(string username, string password)
    {
        var user = await _userRepository.GetByUsername(username);
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
    public async Task Register(string username, string password)
    {
        await _userRepository.Save(new User(Guid.CreateVersion7(), username, PasswordHasher.HashPassword(password)));
    }
}