// FitnessTracker.Api/Dtos/RegisterRequestDto.cs
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Api.Dtos
{
    public class RegisterRequestDto
    {
        [Required]
        [MaxLength(50)]
        public string KullaniciAdi { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [EmailAddress] // E-posta formatını kontrol eder
        public string Eposta { get; set; } = null!;

        [Required]
        [MinLength(6)] // Şifre uzunluk kontrolü
        public string Sifre { get; set; } = null!;

        [MaxLength(50)]
        public string? Ad { get; set; }

        [MaxLength(50)]
        public string? Soyad { get; set; }

        // Rol alanı buraya eklendi
        [Required] // Rol alanı da zorunlu olsun
        [MaxLength(20)]
        public string Rol { get; set; } = "Danisan"; // Varsayılan rolü Danisan olarak ayarladık
    }
}