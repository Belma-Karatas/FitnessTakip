// FitnessTracker.Api/Services/AuthenticationService.cs
using BCrypt.Net;

namespace FitnessTracker.Api.Services // Bu namespace doğru olmalı
{
    public class AuthenticationService : IAuthenticationService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string providedPassword, string hashedPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
            }
            catch (SaltParseException)
            {
                return false;
            }
        }
    }
}