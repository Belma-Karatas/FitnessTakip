// FitnessTracker.Api/Models/Antrenman.cs

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.Api.Models
{
    public class Antrenman
    {
        [Key]
        public int AntrenmanID { get; set; }

        [ForeignKey("Kullanici")]
        public int KullaniciID { get; set; }

        public DateTime AntrenmanTarihi { get; set; }

        public string? Notlar { get; set; }

        // Navigation Property: EF Core tarafından doldurulacağı için null olmayacağını garanti ediyoruz.
        public Kullanici Kullanici { get; set; } = null!;
    }
}