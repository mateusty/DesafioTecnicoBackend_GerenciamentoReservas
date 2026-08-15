namespace Domain.Identity;

public interface IUserRepository
{
	Task<User?> GetbyId(Guid id);
	Task<User?> GetByEmail(string email);
	Task Save(User user);
}