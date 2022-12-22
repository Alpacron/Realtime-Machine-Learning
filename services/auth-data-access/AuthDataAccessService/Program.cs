using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using AuthDataAccessService.Data;
using AuthDataAccessService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("secrets/appsettings.secrets.json", true);

// Add db connection

var conStrBuilder = new MySqlConnectionStringBuilder(
    builder.Configuration.GetConnectionString("DBConnectionString"));
var connection = conStrBuilder.ConnectionString;

builder.Services.AddDbContext<AuthContext>(
    options => options.UseMySql(connection, ServerVersion.AutoDetect(connection)));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IDataAccessService, DataAccessService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/healthz");

app.Run();
