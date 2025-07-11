namespace FitnessTracker.Api.Dtos
{
    public class LoginResponseDto
    {
        public int KullaniciID { get; set; }
        public string KullaniciAdi { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public string Token { get; set; } = null!; // JWT Token
    }
}