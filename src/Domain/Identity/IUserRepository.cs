namespace Domain.Identity;

public interface IUserRepository
{
	Task<User?> GetByUsername(string username);
	Task Save(User user);
}