// FitnessTracker.Api/Program.cs

using FitnessTracker.Api.Data; // ApplicationDbContext i�in
using FitnessTracker.Api.Models; // Kullanici modeli i�in
using FitnessTracker.Api.Dtos; // DTO'lar i�in
using Microsoft.EntityFrameworkCore; // DbContext i�in
using Microsoft.Extensions.DependencyInjection; // AddScoped, AddDbContext gibi extension metotlar i�in
using Microsoft.Extensions.Hosting; // Host builder i�in
using BCrypt.Net; // BCrypt k�t�phanesi i�in
using Microsoft.AspNetCore.Authentication.JwtBearer; // JWT Bearer i�in
using Microsoft.IdentityModel.Tokens; // Token ve SecurityKey i�in
using System.Text; // Encoding i�in
using System.Security.Claims; // Claims i�in
using System.IdentityModel.Tokens.Jwt; // JwtSecurityTokenHandler i�in
using FitnessTracker.Api.Services; // IAuthenticationService ve AuthenticationService i�in gerekli using ifadesi

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 1. DbContext Servisini Kaydetme
// Veritaban� ba�lant� c�mlesi appsettings.json'dan al�n�r.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// E�er connection string'i bulamazsa hata f�rlat
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Controllers Servisini Kaydetme
builder.Services.AddControllers();

// Swagger/OpenAPI ayarlar�
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS ayarlar� (React frontend'i i�in)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", // Politika ad�
        policy => policy.WithOrigins("http://localhost:5173") // Frontend'in �al��t��� URL
                        .AllowAnyMethod() // T�m HTTP metotlar�na izin ver (GET, POST, PUT, DELETE vb.)
                        .AllowAnyHeader() // T�m request header'lar�na izin ver
                        .AllowCredentials()); // Cookies veya Authorization ba�l�klar�n�n g�nderilmesine izin ver (JWT i�in �nemli)
});

// 3. Authentication Service'i Kaydetme
// Scoped, her istek i�in yeni bir instance olu�turur. Singleton da olabilir ama Scoped daha yayg�n kullan�l�r.
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// --- JWT Authentication Ayarlar� ---
var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettingsSection["SecretKey"];
var issuer = jwtSettingsSection["Issuer"];
var audience = jwtSettingsSection["Audience"];

// SecretKey'in bo� olmad���ndan emin olal�m, aksi halde uygulama ba�lamaz.
if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
{
    // Bu ayarlar�n appsettings.json dosyas�nda do�ru bir �ekilde tan�mland���ndan emin olun.
    throw new InvalidOperationException("JWT settings (SecretKey, Issuer, Audience) are not configured correctly in appsettings.json.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Token do�rulama gereklilikleri
            ValidateIssuer = true,          // Token yay�nc�s�n� do�rula
            ValidateAudience = true,        // Token al�c�s�n� do�rula
            ValidateLifetime = true,        // Token ge�erlilik s�resini do�rula
            ValidateIssuerSigningKey = true, // Token imzalama anahtar�n� do�rula

            // appsettings.json'dan gelen de�erler
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)) // G�venlik anahtar� byte array'e �evrilir
        };
    });

// 4. Authorization'� Aktif Etme
// Bu, [Authorize] attribute'�n�n �al��mas� i�in gereklidir.
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Swagger UI'y� etkinle�tirir (/swagger adresinde)
}

// HTTPS redirection'� etkinle�tirme (e�er HTTPS kullan�l�yorsa ve sertifika varsa)
// app.UseHttpsRedirection();

// CORS politikas�n� kullanmaya ba�la
app.UseCors("AllowSpecificOrigin");

// Kimlik do�rulama middleware'�n� kullanmaya ba�la (JWT'yi do�rulamak i�in)
// Bu middleware, gelen istekteki token'� kontrol eder ve do�rulayarak kullan�c� kimli�ini belirler.
app.UseAuthentication();

// Yetkilendirme middleware'�n� kullanmaya ba�la (Kullan�c�n�n belirli bir kayna�a eri�im izni var m� kontrol eder)
app.UseAuthorization();

// Controller'lardaki endpoint'leri �al��t�r�r
app.MapControllers();

app.Run();