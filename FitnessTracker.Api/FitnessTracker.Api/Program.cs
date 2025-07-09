using FitnessTracker.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Veritabaný baðlantý cümlesini appsettings.json dosyasýndan okuma
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add services to the container.
// DbContext'i servislere ekleme ve hangi veritabaný saðlayýcýsýný ve baðlantý cümlesini kullanacaðýný belirtme
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// CORS (Cross-Origin Resource Sharing) ayarlarý
app.UseCors(policy =>
    policy.WithOrigins("http://localhost:5173") // React uygulamasýnýn çalýþtýðý adres
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
