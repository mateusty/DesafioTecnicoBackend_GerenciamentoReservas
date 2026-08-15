using System.Data;

using Microsoft.Extensions.Configuration;
using Npgsql;
using Dapper;
using Domain.Identity;

namespace Infrastructure.Identity;

public class UserRepository : IUserRepository
{
	private readonly string _connectionString;

	public UserRepository(IConfiguration configuration)
	{
        _connectionString = configuration.GetConnectionString("DefaultConnection");
	}

	public async Task<User?> GetbyId(Guid id)
	{
        const string sql = """
            SELECT id, email, password_hash AS PasswordHash
            FROM users
            WHERE id = @Id
        """;

        using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QuerySingleOrDefaultAsync<User>(
            sql,
            new { Id = id }
        );
    }

	public async Task<User?> GetByEmail(string email)
	{
		const string sql = """
            SELECT id, email, password_hash AS PasswordHash
            FROM users
            WHERE email = @Email
        """;

		using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QuerySingleOrDefaultAsync<User>(
			sql,
			new { Email = email }
		);
	}

	public async Task Save(User user)
	{
		const string sql = """
            INSERT INTO users (id, email, password_hash)
            VALUES (@Id, @Email, @PasswordHash)
        """;

		using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(sql, user);
	}
}