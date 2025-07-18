// FitnessTracker.Api/FitnessTracker.Api/Models/Kullanici.cs
using System;
using System.Collections.Generic; // ICollection için gerekli
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
        public string KullaniciAdi { get; set; } = null!; // CS8618 giderildi

        [Required]
        [MaxLength(100)]
        public string Eposta { get; set; } = null!; // CS8618 giderildi

        [Required]
        public string SifreHash { get; set; } = null!; // CS8618 giderildi

        [MaxLength(50)]
        public string? Ad { get; set; } // Null olabilir

        [MaxLength(50)]
        public string? Soyad { get; set; } // Null olabilir

        [MaxLength(10)]
        public string? Cinsiyet { get; set; } // Null olabilir

        public DateTime? DogumTarihi { get; set; } // Null olabilir

        public int? BoyCM { get; set; } // Null olabilir

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? GuncelKiloKG { get; set; } // Null olabilir

        public DateTime OlusturulmaTarihi { get; set; } = DateTime.UtcNow; // Varsayılan değer atandığı için uyarı yok

        [Required]
        [MaxLength(20)]
        public string Rol { get; set; } = "Danisan"; // CS8618 giderildi, varsayılan değer de var

        public int? KocID { get; set; } // Null olabilir.

        // --- Navigation Properties ---
        // Bu özellikler, Entity Framework'ün tablolar arasındaki ilişkileri anlamasına yardımcı olur.
        // "virtual" anahtar kelimesi, lazy loading (tembel yükleme) için kullanılır.

        // Bir kullanıcının (antrenörün) danışanları.
        // Bu, bir antrenörün kendi danışanlarını listelemek istediğimizde işe yarar.
        // Örn: Antrenörsün KOCID'si ile bu listeyi doldurabiliriz.
        public virtual ICollection<Kullanici> Danisanlar { get; set; } = new List<Kullanici>();

        // Bir kullanıcının (danışanın) kendi antrenörü.
        // Bu, danışanın hangi antrenöre bağlı olduğunu belirtir.
        [ForeignKey("KocID")] // KocID alanına bağlı olduğunu belirtir.
        public virtual Kullanici? Koc { get; set; } // Antrenörü null olabilir.

        // Bir kullanıcının (hem antrenör hem danışan olabilir) yaptığı antrenmanlar.
        // Danışanlar kendi antrenmanlarını görmek istediğinde bu liste kullanılacak.
        public virtual ICollection<Antrenman> Antrenmanlar { get; set; } = new List<Antrenman>();

        // Bir kullanıcının (danışan) sahip olduğu hedefler.
        public virtual ICollection<Hedef> Hedefler { get; set; } = new List<Hedef>();

        // --- Navigation Properties Sonu ---
    }
}