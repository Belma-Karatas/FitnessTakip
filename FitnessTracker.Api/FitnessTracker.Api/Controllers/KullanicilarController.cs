// FitnessTracker.Api/Controllers/KullanicilarController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessTracker.Api.Data;
using FitnessTracker.Api.Models;
using FitnessTracker.Api.Dtos; // DTO'ları kullanmak için bu namespace'i ekleyin
using Microsoft.AspNetCore.Authorization; // Authorize attribute'ü için
using System.Security.Claims; // ClaimsPrincipal için

namespace FitnessTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Bu endpoint'e erişim için giriş yapmış olmak gereklidir. JWT token'ı header'da olmalı.
    public class KullanicilarController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public KullanicilarController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Giriş yapmış antrenörün kendi danışanlarını listeler.
        /// Sadece Antrenör rolündeki kullanıcılar bu metodu çağırabilir.
        /// </summary>
        /// <returns>Danışanların listesi.</returns>
        [HttpGet("my-clients")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ClientListDto>))] // Başarılı yanıt tipi
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Yetkisiz erişim (Giriş yapılmamışsa)
        [ProducesResponseType(StatusCodes.Status403Forbidden)]    // Yetkiniz yok (Giriş yapılmış ama rol farklıysa)
        [ProducesResponseType(StatusCodes.Status404NotFound)]     // Kaynak bulunamadı (örn: antrenöre ait danışan yoksa)
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Sunucu hatası

        // Bu endpoint'in sadece "Antrenor" rolündeki kullanıcılar tarafından erişilebilir olmasını sağla.
        [Authorize(Roles = "Antrenor")]
        public async Task<ActionResult<IEnumerable<ClientListDto>>> GetMyClients()
        {
            // JWT token'dan mevcut kullanıcının ID'sini alalım.
            // ClaimTypes.NameIdentifier genellikle kullanıcının ID'sini tutar.
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            // Kullanıcı kimliği alınamazsa veya geçerli değilse 401 Unauthorized döndür.
            // [Authorize] attribute'ü zaten bunu bir dereceye kadar halleder, ama yine de kontrol etmek iyi.
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
            {
                // Kullanıcı ID'si alınamazsa veya geçersizse yetkisiz döndür
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            // Not: Rol kontrolü [Authorize(Roles = "Antrenor")] attribute'ü ile zaten yapılmıştır.
            // Bu nedenle burada ayrıca rolü kontrol etmeye gerek yoktur, ancak isterseniz yapabilirsiniz:
            // var roleClaim = User.FindFirst(ClaimTypes.Role);
            // if (roleClaim == null || roleClaim.Value != "Antrenor")
            // {
            //     return Forbid("Bu işlemi gerçekleştirmek için yetkiniz bulunmamaktadır.");
            // }


            // Sadece kullanıcının (antrenörün) KocID'si olan danışanları filtrele
            var clients = await _context.Kullanicilar
                .Where(u => u.KocID == currentUserId) // Sadece bu kullanıcının koç olduğu danışanları getir
                                                      // DTO'ya dönüştürmeden önce gerekli alanları seçelim
                .Select(u => new ClientListDto
                {
                    KullaniciID = u.KullaniciID,
                    KullaniciAdi = u.KullaniciAdi,
                    Ad = u.Ad,
                    Soyad = u.Soyad,
                    Eposta = u.Eposta,
                    GuncelKiloKG = u.GuncelKiloKG,
                    Rol = u.Rol // Rolünü de göstermek isteyebiliriz
                })
                .ToListAsync();

            // Eğer hiç danışan bulunamazsa 404 NotFound döndür.
            if (clients == null || clients.Count == 0)
            {
                // Bu antrenöre ait hiç danışan bulunamadı mesajını kullanıcıya göster.
                return NotFound("Bu antrenöre ait hiç danışan bulunamadı.");
            }

            // Danışanlar başarıyla getirildiyse 200 OK ile listeyi döndür.
            return Ok(clients);
        }

        // Buraya diğer KullanicilarController metodları gelebilir (örn: GetUserById, UpdateUser vb.)
        // Örneğin:
        /*
        [HttpGet("{id}")]
        [Authorize(Roles = "Antrenor, Danisan")] // Hem Antrenör hem de Danışan kendi profilini görebilir
        public async Task<ActionResult<UserDetailDto>> GetUserById(int id)
        {
            var currentUser = HttpContext.User;
            var currentUserId = int.Parse(currentUser.FindFirst(ClaimTypes.NameIdentifier).Value);
            var currentUserRole = currentUser.FindFirst(ClaimTypes.Role)?.Value;

            // Eğer kullanıcı kendi profilini görmeye çalışıyorsa veya antrenör ise
            if (currentUserRole == "Antrenor" || currentUserId == id)
            {
                var user = await _context.Kullanicilar
                    .Where(u => u.KullaniciID == id)
                    .Select(u => new UserDetailDto // UserDetailDto'yu oluşturmanız gerekebilir
                    {
                        KullaniciID = u.KullaniciID,
                        KullaniciAdi = u.KullaniciAdi,
                        Ad = u.Ad,
                        Soyad = u.Soyad,
                        Eposta = u.Eposta,
                        Cinsiyet = u.Cinsiyet,
                        DogumTarihi = u.DogumTarihi,
                        BoyCM = u.BoyCM,
                        GuncelKiloKG = u.GuncelKiloKG,
                        Rol = u.Rol
                        // İleride koç bilgileri de eklenebilir (eğer danışan kendi koçunu görmek isterse)
                    })
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound($"Kullanıcı bulunamadı.");
                }
                return Ok(user);
            }
            else
            {
                return Forbid("Bu bilgiye erişim izniniz yok.");
            }
        }
        */
    }
}