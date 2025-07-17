// FitnessTracker.Api/Services/IAuthenticationService.cs
namespace FitnessTracker.Api.Services // Bu namespace doğru olmalı
{
    // Şifreleme ve doğrulama işlemlerini soyutlayan interface
    public interface IAuthenticationService
    {
        string HashPassword(string password); // Şifreyi güvenli bir şekilde hash'ler
        bool VerifyPassword(string providedPassword, string hashedPassword); // Sağlanan şifreyi hash'lenmiş şifreyle karşılaştırır
    }
}