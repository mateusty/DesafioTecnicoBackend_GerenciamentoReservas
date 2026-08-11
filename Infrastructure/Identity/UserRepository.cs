using System.Data;
using Dapper;
using DesafioTecnicoBackend_GerenciamentoReservas.Domain.Identity;

namespace DesafioTecnicoBackend_GerenciamentoReservas.Infrastructure.Identity;

public class UserRepository : IUserRepository
{
	private readonly IDbConnection _connection;

	public UserRepository(IDbConnection connection)
	{
		_connection = connection;
	}

	public async Task<User?> GetByUsername(string username)
	{
		const string sql = """
            SELECT id, username, password_hash AS PasswordHash
            FROM users
            WHERE username = @Username
        """;

		return await _connection.QuerySingleOrDefaultAsync<User>(
			sql,
			new { Username = username }
		);
	}

	public async Task Save(User user)
	{
		const string sql = """
            INSERT INTO users (id, username, password_hash)
            VALUES (@Id, @Username, @PasswordHash)
        """;

		await _connection.ExecuteAsync(sql, user);
	}
}