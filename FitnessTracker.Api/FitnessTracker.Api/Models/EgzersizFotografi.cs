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
        public string FotoURL { get; set; }

        [MaxLength(100)]
        public string? Aciklama { get; set; }

        public int SiraNo { get; set; } = 1;

        // Navigation Property - Bu fotoğrafın hangi egzersize ait olduğunu belirtir.
        public Egzersiz Egzersiz { get; set; }
    }
}