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

        // İsteğe bağlı olarak ekleyebilirsiniz:
        // public string? Cinsiyet { get; set; }
        // public DateTime? DogumTarihi { get; set; }
    }
}