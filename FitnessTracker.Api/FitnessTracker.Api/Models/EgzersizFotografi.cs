// FitnessTracker.Api/FitnessTracker.Api/Models/EgzersizFotografi.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.Api.Models
{
    public class EgzersizFotografi
    {
        [Key]
        public int FotografID { get; set; }

        [ForeignKey("Egzersiz")]
        public int EgzersizID { get; set; }

        [Required]
        [MaxLength(255)]
        // CS8618 Uyarısını gidermek için başlatıcı (initializer) eklendi.
        public string FotoURL { get; set; } = null!;

        [MaxLength(100)]
        public string? Aciklama { get; set; } // Null olabilir.

        public int SiraNo { get; set; } = 1; // Varsayılan değer atandığı için uyarı vermez.

        // Navigation Property
        public Egzersiz Egzersiz { get; set; } = null!; // Null olmamalı
    }
}