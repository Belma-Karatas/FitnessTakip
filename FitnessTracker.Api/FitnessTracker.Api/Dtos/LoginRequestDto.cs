using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Api.Dtos
{
    public class LoginRequestDto
    {
        [Required]
        public string KullaniciAdiVeyaEposta { get; set; } = null!; // Kullanıcı adı veya e-posta olabilir

        [Required]
        public string Sifre { get; set; } = null!;
    }
}