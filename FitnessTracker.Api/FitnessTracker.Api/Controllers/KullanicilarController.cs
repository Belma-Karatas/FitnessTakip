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
    [Authorize] // Bu endpoint'e erişim için giriş yapmış olmak gereklidir
    public class KullanicilarController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public KullanicilarController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Giriş yapmış antrenörün kendi danışanlarını listeler.
        /// </summary>
        /// <returns>Danışanların listesi.</returns>
        [HttpGet("my-clients")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ClientListDto>))] // Başarılı yanıt tipi
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Yetkisiz erişim
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Antrenör bulunamazsa veya danışanı yoksa
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Sunucu hatası
        public async Task<ActionResult<IEnumerable<ClientListDto>>> GetMyClients()
        {
            // JWT token'dan mevcut kullanıcının ID'sini alalım.
            // ClaimTypes.NameIdentifier genellikle kullanıcının ID'sini tutar.
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
            {
                // Kullanıcı ID'si alınamazsa veya geçersizse yetkisiz döndür
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            // Kullanıcının gerçekten bir antrenör olup olmadığını da kontrol edebiliriz,
            // ancak şu anlık sadece KocID'si olanları değil, tüm danışanları getiriyoruz.
            // Eğer sadece "KocID" sahibi olanları getireceksek, sorguyu buna göre güncelleriz.
            // Şimdilik, genel bir kullanıcı listesi alıp, KocID'si mevcut kullanıcının ID'si olanları filtreleyelim.

            var clients = await _context.Kullanicilar
                .Where(u => u.KocID == currentUserId)
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

            if (clients == null || clients.Count == 0)
            {
                // Danışan bulunamazsa veya danışanı yoksa 404 döndür
                // Alternatif olarak boş liste de döndürülebilir, bu duruma göre karar verilir.
                // return Ok(clients); // Boş liste döndürmek için
                return NotFound("Bu antrenöre ait hiç danışan bulunamadı.");
            }

            return Ok(clients);
        }

        // Buraya diğer KullanicilarController metodları gelebilir (örn: GetUserById, UpdateUser vb.)
    }
}
