using System.Data;

using Npgsql;
using Dapper;

using DesafioTecnicoBackend_GerenciamentoReservas.Domain.Booking;
using DesafioTecnicoBackend_GerenciamentoReservas.Application.Booking;

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
            SELECT id, name, country, city, address, price_per_night AS PricePerNight
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
            SELECT id, name, country, city, address, price_per_night AS PricePerNight
            FROM hotels
        """;

        using var connection = new NpgsqlConnection(_connectionString);
        return (await connection.QueryAsync<Hotel>(sql)).ToList();
    }

    public async Task Save(HotelRequest hotel)
    {
        const string sql = """
            INSERT INTO hotels (name, country, city, address, price_per_night)
            VALUES (@Name, @Country, @City, @Address, @PricePerNight)
        """;

        using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(sql, hotel);
    }
}