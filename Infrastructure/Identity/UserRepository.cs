using System.Data;

using Npgsql;
using Dapper;
using DesafioTecnicoBackend_GerenciamentoReservas.Domain.Identity;

namespace DesafioTecnicoBackend_GerenciamentoReservas.Infrastructure.Identity;

public class UserRepository : IUserRepository
{
	private readonly string _connectionString;

	public UserRepository(IConfiguration configuration)
	{
        _connectionString = configuration.GetConnectionString("DefaultConnection");
	}

	public async Task<User?> GetByUsername(string username)
	{
		const string sql = """
            SELECT id, username, password_hash AS PasswordHash
            FROM users
            WHERE username = @Username
        """;

		using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QuerySingleOrDefaultAsync<User>(
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

		using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(sql, user);
	}
}