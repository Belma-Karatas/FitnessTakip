// FitnessTracker.Api/Models/IlerlemeTakibi.cs

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.Api.Models
{
    public class IlerlemeTakibi
    {
        [Key]
        public int IlerlemeID { get; set; }

        [ForeignKey("Kullanici")]
        public int KullaniciID { get; set; }

        public DateTime Tarih { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? KiloKG { get; set; }

        [Column(TypeName = "decimal(4, 2)")]
        public decimal? YagOrani { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? KasKutlesiKG { get; set; }

        // Navigation Property - Bu kaydın hangi kullanıcıya ait olduğunu belirtir.
        // CS8618 uyarılarını gidermek için null olmayan değer ataması yapıldı.
        public Kullanici Kullanici { get; set; } = null!;
    }
}