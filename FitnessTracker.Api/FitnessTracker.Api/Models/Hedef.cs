// FitnessTracker.Api/Models/Hedef.cs

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.Api.Models
{
    public class Hedef
    {
        [Key]
        public int HedefID { get; set; }

        [ForeignKey("Kullanici")]
        public int KullaniciID { get; set; }

        [Required]
        [MaxLength(50)]
        public string HedefTipi { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal HedefDeger { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal BaslangicDegeri { get; set; }

        public DateTime BaslangicTarihi { get; set; }

        public DateTime BitisTarihi { get; set; }

        public bool TamamlandiMi { get; set; }

        // Navigation Property - Bu hedefin hangi kullanıcıya ait olduğunu belirtir.
        // CS8618 uyarılarını gidermek için null olmayan değer ataması yapıldı.
        public Kullanici Kullanici { get; set; } = null!;
    }
}