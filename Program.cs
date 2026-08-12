using System.Data;
using System.Text;

using Npgsql;
using DotNetEnv;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using DesafioTecnicoBackend_GerenciamentoReservas.Application.Identity;
using DesafioTecnicoBackend_GerenciamentoReservas.Application.Booking;
using DesafioTecnicoBackend_GerenciamentoReservas.Infrastructure.Identity;
using DesafioTecnicoBackend_GerenciamentoReservas.Infrastructure.Booking;
using DesafioTecnicoBackend_GerenciamentoReservas.Infrastructure.Security;
using DesafioTecnicoBackend_GerenciamentoReservas.Domain.Booking;
using DesafioTecnicoBackend_GerenciamentoReservas.Domain.Identity;


DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddScoped<AuthService>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Cria a conexão com o banco de dados usando Dapper
builder.Services.AddScoped<IDbConnection>(_ =>
    new NpgsqlConnection(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Registra os repositórios
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

// Configurações de segurança
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(secretKey)
        };
    });
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<HotelService>();
builder.Services.AddScoped<BookingService>();

var app = builder.Build();

app.UsePathBase("/api");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
