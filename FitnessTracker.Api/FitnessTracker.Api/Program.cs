using FitnessTracker.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Veritaban� ba�lant� c�mlesini appsettings.json dosyas�ndan okuma
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add services to the container.
// DbContext'i servislere ekleme ve hangi veritaban� sa�lay�c�s�n� ve ba�lant� c�mlesini kullanaca��n� belirtme
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// CORS (Cross-Origin Resource Sharing) ayarlar�
app.UseCors(policy =>
    policy.WithOrigins("http://localhost:5173") // React uygulamas�n�n �al��t��� adres
          .AllowAnyMethod()
          .AllowAnyHeader());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
