using System.Data;

using Npgsql;
using Dapper;

using DesafioTecnicoBackend_GerenciamentoReservas.Domain.Booking;
using DesafioTecnicoBackend_GerenciamentoReservas.Application.Booking;

namespace DesafioTecnicoBackend_GerenciamentoReservas.Infrastructure.Booking;

public class BookingRepository : IBookingRepository
{
    private readonly string _connectionString;

    public BookingRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<Bookings> GetbyId(int id)
    {
        const string sql = """
            SELECT id, user_id AS UserId, hotel_id AS HotelId, room_number AS RoomNumber, start_date AS StartDate, end_date AS EndDate, status
            FROM bookings
            WHERE id = @Id
        """;
        using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<Bookings>(
            sql,
            new { Id = id }
        );
    }

    public async Task<List<Bookings>> GetbyUser(Guid id)
    {
        const string sql = """
            SELECT id, user_id AS UserId, hotel_id AS HotelId, room_number AS RoomNumber, start_date AS StartDate, end_date AS EndDate, status
            FROM bookings
            WHERE user_id = @Id
        """;
        using var connection = new NpgsqlConnection(_connectionString);
        return (await connection.QueryAsync<Bookings>(
            sql,
            new { Id = id }
        )).ToList();
    }

    public async Task Save(BookingRequest booking, Guid userId)
    {
        const string sql = """
            INSERT INTO bookings (user_id, hotel_id, room_number, start_date, end_date, status)
            VALUES (@UserId, @HotelId, @RoomNumber, @StartDate, @EndDate, @Status)
        """;
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(sql, new 
            { 
                UserId = userId,
                HotelId = booking.HotelId,
                RoomNumber = booking.RoomNumber,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                Status = booking.Status
            });
    }

    public async Task Edit(BookingRequest booking, int id, Guid userId)
    {
        const string sql = """
            UPDATE bookings
            SET user_id = @UserId, hotel_id = @HotelId, room_number = @RoomNumber, start_date = @StartDate, end_date = @EndDate, status = @Status
            WHERE id = @Id
        """;
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(sql, new Bookings(booking, id, userId));
    }
}