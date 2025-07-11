// FitnessTracker.Api/Program.cs

using FitnessTracker.Api.Data;
using FitnessTracker.Api.Models;
using FitnessTracker.Api.Dtos; // DTO'lar için
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BCrypt.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.IdentityModel.Tokens; // Token ve SecurityKey için
using System.Text; // Encoding için
using System.Security.Claims; // Claims için
using System.IdentityModel.Tokens.Jwt; // JwtSecurityTokenHandler için

var builder = WebApplication.CreateBuilder(args);

// Veritabaný baðlantý cümlesi
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS ayarlarý (React için)
builder.Services.AddCors(options => // Cors ayarlarý için AddCors kullanýyoruz
{
    options.AddPolicy("AllowSpecificOrigin", // Politika adý
        policy => policy.WithOrigins("http://localhost:5173")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()); // Gerekirse Credentials izni
});

// Authentication Servisi
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// --- JWT Authentication Ayarlarý ---
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
builder.Services.AddAuthorization(); // Bu satýr genellikle gereklidir

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // HTTPS kullanýyorsan açabilirsin

// CORS politikasýný kullan
app.UseCors("AllowSpecificOrigin");

// Authentication ve Authorization middleware'lerini doðru sýraya koymak önemlidir
app.UseAuthentication(); // Önce authentication
app.UseAuthorization();  // Sonra authorization

app.MapControllers();

app.Run();

// --- Authentication Servisi Interface ve Class (Daha önce tanýmlanan) ---
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