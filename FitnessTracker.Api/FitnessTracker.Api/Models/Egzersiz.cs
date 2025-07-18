// FitnessTracker.Api/FitnessTracker.Api/Models/Egzersiz.cs
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Api.Models
{
    public class Egzersiz
    {
        [Key]
        public int EgzersizID { get; set; }

        [Required]
        [MaxLength(100)]
        // CS8618 Uyarısını gidermek için başlatıcı (initializer) eklendi.
        public string Ad { get; set; } = null!;

        public string? Aciklama { get; set; } // Null olabilir.

        [Required]
        [MaxLength(50)]
        // CS8618 Uyarısını gidermek için başlatıcı (initializer) eklendi.
        public string KasGrubu { get; set; } = null!;
    }
}