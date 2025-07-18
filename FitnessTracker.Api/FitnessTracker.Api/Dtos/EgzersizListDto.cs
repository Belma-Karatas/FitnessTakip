// FitnessTracker.Api/FitnessTracker.Api/Dtos/EgzersizListDto.cs
namespace FitnessTracker.Api.Dtos
{
    public class EgzersizListDto
    {
        public int EgzersizID { get; set; }
        public string Ad { get; set; } = null!;
        public string KasGrubu { get; set; } = null!;
        // Fotoğraf bilgisi de eklenebilir
    }
}