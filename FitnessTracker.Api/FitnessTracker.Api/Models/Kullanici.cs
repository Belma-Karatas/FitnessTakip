using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.Api.Models
{
    public class Kullanici
    {
        [Key]
        public int KullaniciID { get; set; }

        [Required]
        [MaxLength(50)]
        public string KullaniciAdi { get; set; }

        [Required]
        [MaxLength(100)]
        public string Eposta { get; set; }

        [Required]
        public string SifreHash { get; set; }

        [MaxLength(50)]
        public string? Ad { get; set; }

        [MaxLength(50)]
        public string? Soyad { get; set; }

        [MaxLength(10)]
        public string? Cinsiyet { get; set; }

        public DateTime? DogumTarihi { get; set; }

        public int? BoyCM { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? GuncelKiloKG { get; set; }

        public DateTime OlusturulmaTarihi { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(20)]
        public string Rol { get; set; } = "Danisan";

        public int? KocID { get; set; }
    }
}