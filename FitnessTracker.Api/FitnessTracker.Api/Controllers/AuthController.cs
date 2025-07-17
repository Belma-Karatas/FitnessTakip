// FitnessTracker.Api/Controllers/AuthController.cs

using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Api.Models;
using FitnessTracker.Api.Data;
using FitnessTracker.Api.Dtos;
using FitnessTracker.Api.Services; // 🔴 IAuthenticationService için gerekli
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FitnessTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthenticationService _authService;
        private readonly IConfiguration _configuration; // JWT Ayarları için

        public AuthController(ApplicationDbContext context, IAuthenticationService authService, IConfiguration configuration)
        {
            _context = context;
            _authService = authService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // DTO validasyon hatalarını döndür
            }

            var existingUser = await _context.Kullanicilar.FirstOrDefaultAsync(u => u.KullaniciAdi == request.KullaniciAdi || u.Eposta == request.Eposta);
            if (existingUser != null)
            {
                return Conflict("Kullanıcı adı veya e-posta zaten kullanılıyor.");
            }

            var hashedPassword = _authService.HashPassword(request.Sifre);

            var newUser = new Kullanici
            {
                KullaniciAdi = request.KullaniciAdi,
                Eposta = request.Eposta,
                SifreHash = hashedPassword,
                Ad = request.Ad,
                Soyad = request.Soyad,
                OlusturulmaTarihi = DateTime.UtcNow,
                Rol = request.Rol
            };

            _context.Kullanicilar.Add(newUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Register), new { id = newUser.KullaniciID }, new { message = "Kullanıcı başarıyla kaydedildi." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Kullanicilar.FirstOrDefaultAsync(u => u.KullaniciAdi == request.KullaniciAdiVeyaEposta || u.Eposta == request.KullaniciAdiVeyaEposta);

            if (user == null)
            {
                return Unauthorized("Geçersiz kullanıcı adı veya şifre.");
            }

            if (!_authService.VerifyPassword(request.Sifre, user.SifreHash))
            {
                return Unauthorized("Geçersiz kullanıcı adı veya şifre.");
            }

            var token = GenerateJwtToken(user);

            return Ok(new LoginResponseDto
            {
                KullaniciID = user.KullaniciID,
                KullaniciAdi = user.KullaniciAdi,
                Rol = user.Rol,
                Token = token
            });
        }

        private string GenerateJwtToken(Kullanici user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.KullaniciID.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.KullaniciAdi),
                new Claim(ClaimTypes.Email, user.Eposta),
                new Claim(ClaimTypes.Role, user.Rol)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}

