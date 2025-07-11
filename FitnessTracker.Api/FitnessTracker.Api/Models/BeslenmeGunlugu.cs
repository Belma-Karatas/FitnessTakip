// FitnessTracker.Api/Models/BeslenmeGunlugu.cs

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.Api.Models
{
    public class BeslenmeGunlugu
    {
        [Key]
        public int BeslenmeID { get; set; }

        [ForeignKey("Kullanici")]
        public int KullaniciID { get; set; }

        public DateTime KayitTarihi { get; set; }

        [Required]
        [MaxLength(50)]
        public string OgunTipi { get; set; }

        [Required]
        [MaxLength(100)]
        public string YiyecekAdi { get; set; }

        public int Kalori { get; set; }

        [Column(TypeName = "decimal(5, 1)")]
        public decimal Protein { get; set; }

        [Column(TypeName = "decimal(5, 1)")]
        public decimal Karbonhidrat { get; set; }

        [Column(TypeName = "decimal(5, 1)")]
        public decimal Yag { get; set; }

        // Navigation Property - Bu kaydın hangi kullanıcıya ait olduğunu belirtir.
        // CS8618 uyarılarını gidermek için null olmayan değer ataması yapıldı.
        public Kullanici Kullanici { get; set; } = null!;
    }
}