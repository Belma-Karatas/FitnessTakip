// FitnessTracker.Api/Program.cs

using FitnessTracker.Api.Data;
using FitnessTracker.Api.Models;
using FitnessTracker.Api.Dtos; // DTO'lar i�in
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BCrypt.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.IdentityModel.Tokens; // Token ve SecurityKey i�in
using System.Text; // Encoding i�in
using System.Security.Claims; // Claims i�in
using System.IdentityModel.Tokens.Jwt; // JwtSecurityTokenHandler i�in

var builder = WebApplication.CreateBuilder(args);

// Veritaban� ba�lant� c�mlesi
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS ayarlar� (React i�in)
builder.Services.AddCors(options => // Cors ayarlar� i�in AddCors kullan�yoruz
{
    options.AddPolicy("AllowSpecificOrigin", // Politika ad�
        policy => policy.WithOrigins("http://localhost:5173")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()); // Gerekirse Credentials izni
});

// Authentication Servisi
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// --- JWT Authentication Ayarlar� ---
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey ?? throw new InvalidOperationException("JwtSettings:SecretKey cannot be null.")))
        };
    });

// Authorization
builder.Services.AddAuthorization(); // Bu sat�r genellikle gereklidir

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // HTTPS kullan�yorsan a�abilirsin

// CORS politikas�n� kullan
app.UseCors("AllowSpecificOrigin");

// Authentication ve Authorization middleware'lerini do�ru s�raya koymak �nemlidir
app.UseAuthentication(); // �nce authentication
app.UseAuthorization();  // Sonra authorization

app.MapControllers();

app.Run();

// --- Authentication Servisi Interface ve Class (Daha �nce tan�mlanan) ---
public interface IAuthenticationService
{
    string HashPassword(string password);
    bool VerifyPassword(string providedPassword, string hashedPassword);
}

public class AuthenticationService : IAuthenticationService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
    }

    public bool VerifyPassword(string providedPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
    }
}