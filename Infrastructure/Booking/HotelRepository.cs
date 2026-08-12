using System.Data;

using Npgsql;
using Dapper;

using DesafioTecnicoBackend_GerenciamentoReservas.Domain.Booking;

namespace DesafioTecnicoBackend_GerenciamentoReservas.Infrastructure.Booking;

public class HotelRepository : IHotelRepository
{
    private readonly string _connectionString;

    public HotelRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<Hotel?> GetbyId(int id)
    {
        const string sql = """
            SELECT id, name, country, city, address, rating, price_per_night
            FROM hotels
            WHERE id = @Id
        """;

        using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QuerySingleOrDefaultAsync<Hotel>(
            sql,
            new { Id = id }
        );
    }

    public async Task<List<Hotel>> GetAll()
    {
        const string sql = """
            SELECT id, name, country, city, address, rating, price_per_night
            FROM hotels
        """;

        using var connection = new NpgsqlConnection(_connectionString);
        return (await connection.QueryAsync<Hotel>(sql)).ToList();
    }

    public async Task Save(Hotel hotel)
    {
        const string sql = """
            INSERT INTO hotels (id, name, country, city, address, rating, price_per_night)
            VALUES (@Id, @Name, @Country, @City, @Address, @Rating, @PricePerNight)
        """;

        using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(sql, hotel);
    }
}