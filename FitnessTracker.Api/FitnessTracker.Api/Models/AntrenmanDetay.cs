// FitnessTracker.Api/Models/AntrenmanDetay.cs

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.Api.Models
{
    public class AntrenmanDetay
    {
        [Key]
        public int DetayID { get; set; }

        [ForeignKey("Antrenman")]
        public int AntrenmanID { get; set; }

        [ForeignKey("Egzersiz")]
        public int EgzersizID { get; set; }

        public int SetSayisi { get; set; }

        public int TekrarSayisi { get; set; }

        [Column(TypeName = "decimal(6, 2)")]
        public decimal Agirlik { get; set; }

        // Navigation Properties - Bu kaydın hangi antrenmana ve egzersize bağlı olduğunu belirtir.
        // CS8618 uyarılarını gidermek için null olmayan değer ataması yapıldı.
        public Antrenman Antrenman { get; set; } = null!;
        public Egzersiz Egzersiz { get; set; } = null!;
    }
}