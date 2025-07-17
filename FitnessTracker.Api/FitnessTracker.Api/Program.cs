// FitnessTracker.Api/Program.cs

using FitnessTracker.Api.Data; // ApplicationDbContext için
using FitnessTracker.Api.Models; // Kullanici modeli için
using FitnessTracker.Api.Dtos; // DTO'lar için
using Microsoft.EntityFrameworkCore; // DbContext için
using Microsoft.Extensions.DependencyInjection; // AddScoped, AddDbContext gibi extension metotlar için
using Microsoft.Extensions.Hosting; // Host builder için
using BCrypt.Net; // BCrypt kütüphanesi için
using Microsoft.AspNetCore.Authentication.JwtBearer; // JWT Bearer için
using Microsoft.IdentityModel.Tokens; // Token ve SecurityKey için
using System.Text; // Encoding için
using System.Security.Claims; // Claims için
using System.IdentityModel.Tokens.Jwt; // JwtSecurityTokenHandler için
using FitnessTracker.Api.Services; // IAuthenticationService ve AuthenticationService için gerekli using ifadesi

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 1. DbContext Servisini Kaydetme
// Veritabaný baðlantý cümlesi appsettings.json'dan alýnýr.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Eðer connection string'i bulamazsa hata fýrlat
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Controllers Servisini Kaydetme
builder.Services.AddControllers();

// Swagger/OpenAPI ayarlarý
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS ayarlarý (React frontend'i için)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", // Politika adý
        policy => policy.WithOrigins("http://localhost:5173") // Frontend'in çalýþtýðý URL
                        .AllowAnyMethod() // Tüm HTTP metotlarýna izin ver (GET, POST, PUT, DELETE vb.)
                        .AllowAnyHeader() // Tüm request header'larýna izin ver
                        .AllowCredentials()); // Cookies veya Authorization baþlýklarýnýn gönderilmesine izin ver (JWT için önemli)
});

// 3. Authentication Service'i Kaydetme
// Scoped, her istek için yeni bir instance oluþturur. Singleton da olabilir ama Scoped daha yaygýn kullanýlýr.
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// --- JWT Authentication Ayarlarý ---
var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettingsSection["SecretKey"];
var issuer = jwtSettingsSection["Issuer"];
var audience = jwtSettingsSection["Audience"];

// SecretKey'in boþ olmadýðýndan emin olalým, aksi halde uygulama baþlamaz.
if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
{
    // Bu ayarlarýn appsettings.json dosyasýnda doðru bir þekilde tanýmlandýðýndan emin olun.
    throw new InvalidOperationException("JWT settings (SecretKey, Issuer, Audience) are not configured correctly in appsettings.json.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Token doðrulama gereklilikleri
            ValidateIssuer = true,          // Token yayýncýsýný doðrula
            ValidateAudience = true,        // Token alýcýsýný doðrula
            ValidateLifetime = true,        // Token geçerlilik süresini doðrula
            ValidateIssuerSigningKey = true, // Token imzalama anahtarýný doðrula

            // appsettings.json'dan gelen deðerler
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)) // Güvenlik anahtarý byte array'e çevrilir
        };
    });

// 4. Authorization'ý Aktif Etme
// Bu, [Authorize] attribute'ünün çalýþmasý için gereklidir.
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Swagger UI'yý etkinleþtirir (/swagger adresinde)
}

// HTTPS redirection'ý etkinleþtirme (eðer HTTPS kullanýlýyorsa ve sertifika varsa)
// app.UseHttpsRedirection();

// CORS politikasýný kullanmaya baþla
app.UseCors("AllowSpecificOrigin");

// Kimlik doðrulama middleware'ýný kullanmaya baþla (JWT'yi doðrulamak için)
// Bu middleware, gelen istekteki token'ý kontrol eder ve doðrulayarak kullanýcý kimliðini belirler.
app.UseAuthentication();

// Yetkilendirme middleware'ýný kullanmaya baþla (Kullanýcýnýn belirli bir kaynaða eriþim izni var mý kontrol eder)
app.UseAuthorization();

// Controller'lardaki endpoint'leri çalýþtýrýr
app.MapControllers();

app.Run();