using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Api.Models
{
    public class Egzersiz
    {
        [Key]
        public int EgzersizID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Ad { get; set; }

        public string? Aciklama { get; set; }

        [Required]
        [MaxLength(50)]
        public string KasGrubu { get; set; }
    }
}