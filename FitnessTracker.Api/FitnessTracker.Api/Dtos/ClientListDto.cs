// FitnessTracker.Api/Dtos/ClientListDto.cs
namespace FitnessTracker.Api.Dtos
{
    public class ClientListDto
    {
        public int KullaniciID { get; set; }
        public string KullaniciAdi { get; set; } = null!;
        public string? Ad { get; set; }
        public string? Soyad { get; set; }
        public string Eposta { get; set; } = null!;
        public decimal? GuncelKiloKG { get; set; }
        public string Rol { get; set; } = null!; // Kullanıcının rolünü de göstermek isteyebiliriz
        // İhtiyaç duyulursa başka alanlar da eklenebilir
    }
}