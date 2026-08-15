namespace Domain.Identity;

public interface IUserRepository
{
	Task<User?> GetbyId(Guid id);
	Task<User?> GetByUsername(string username);
	Task Save(User user);
}