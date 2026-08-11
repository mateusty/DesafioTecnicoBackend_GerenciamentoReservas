var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Cria a conexão com o banco de dados usando Dapper
builder.Services.AddScoped<IDbConnection>(_ =>
    new NpgsqlConnection(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Registra os repositórios
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
