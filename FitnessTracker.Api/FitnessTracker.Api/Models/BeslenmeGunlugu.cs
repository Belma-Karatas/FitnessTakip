// FitnessTracker.Api/FitnessTracker.Api/Models/BeslenmeGunlugu.cs
using System;
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
        // CS8618 Uyarısını gidermek için başlatıcı (initializer) eklendi.
        public string OgunTipi { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        // CS8618 Uyarısını gidermek için başlatıcı (initializer) eklendi.
        public string YiyecekAdi { get; set; } = null!;

        public int Kalori { get; set; }

        [Column(TypeName = "decimal(5, 1)")]
        public decimal Protein { get; set; }

        [Column(TypeName = "decimal(5, 1)")]
        public decimal Karbonhidrat { get; set; }

        [Column(TypeName = "decimal(5, 1)")]
        public decimal Yag { get; set; }

        // Navigation Property
        public Kullanici Kullanici { get; set; } = null!; // Null olmamalı
    }
}