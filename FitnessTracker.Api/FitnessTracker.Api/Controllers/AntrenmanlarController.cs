// FitnessTracker.Api/FitnessTracker.Api/Controllers/AntrenmanlarController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessTracker.Api.Data;
using FitnessTracker.Api.Dtos;
using FitnessTracker.Api.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Bu endpoint'lere erişim için yetkilendirme gereklidir.
    public class AntrenmanlarController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AntrenmanlarController(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AntrenmanListDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserAntrenmanlari()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var antrenmanList = await _context.Antrenmanlar
                    .Where(a => a.KullaniciID == currentUserId)
                    .Select(a => new AntrenmanListDto
                    {
                        AntrenmanID = a.AntrenmanID,
                        AntrenmanTarihi = a.AntrenmanTarihi,
                        Notlar = a.Notlar != null && a.Notlar.Length > 50 ? a.Notlar.Substring(0, 50) + "..." : a.Notlar,
                        EgzersizSayisi = _context.AntrenmanDetaylari.Count(ad => ad.AntrenmanID == a.AntrenmanID)
                    })
                    .ToListAsync();

                if (antrenmanList == null || !antrenmanList.Any())
                {
                    return Ok(new List<AntrenmanListDto>());
                }

                return Ok(antrenmanList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetUserAntrenmanlari Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Antrenmanlar getirilirken sunucu hatası.");
            }
        }

        [HttpGet("{antrenmanId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AntrenmanDetailDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAntrenmanDetail(int antrenmanId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var antrenman = await _context.Antrenmanlar
                    .Include(a => a.AntrenmanDetaylari)
                    .ThenInclude(ad => ad.Egzersiz)
                    .FirstOrDefaultAsync(a => a.AntrenmanID == antrenmanId && a.KullaniciID == currentUserId);

                if (antrenman == null)
                {
                    return NotFound("Belirtilen antrenman bulunamadı veya bu antrenmana erişim izniniz yok.");
                }

                var antrenmanDetailDto = new AntrenmanDetailDto
                {
                    AntrenmanID = antrenman.AntrenmanID,
                    AntrenmanTarihi = antrenman.AntrenmanTarihi,
                    Notlar = antrenman.Notlar,
                    Detaylar = antrenman.AntrenmanDetaylari.Select(ad => new AntrenmanDetayDto
                    {
                        DetayID = ad.DetayID,
                        EgzersizID = ad.EgzersizID,
                        EgzersizAdi = ad.Egzersiz?.Ad ?? "Bilinmiyor",
                        KasGrubu = ad.Egzersiz?.KasGrubu ?? "Bilinmiyor",
                        SetSayisi = ad.SetSayisi,
                        TekrarSayisi = ad.TekrarSayisi,
                        Agirlik = ad.Agirlik
                    }).ToList()
                };

                return Ok(antrenmanDetailDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetAntrenmanDetail Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Antrenman detayları getirilirken sunucu hatası.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AntrenmanListDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAntrenman([FromBody] CreateAntrenmanDto createAntrenmanDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var newAntrenman = new Antrenman
                {
                    KullaniciID = currentUserId,
                    AntrenmanTarihi = createAntrenmanDto.AntrenmanTarihi,
                    Notlar = createAntrenmanDto.Notlar
                };

                _context.Antrenmanlar.Add(newAntrenman);
                await _context.SaveChangesAsync();

                foreach (var detayDto in createAntrenmanDto.Detaylar)
                {
                    var egzersizExists = await _context.Egzersizler.AnyAsync(e => e.EgzersizID == detayDto.EgzersizID);
                    if (!egzersizExists)
                    {
                        continue;
                    }

                    var newAntrenmanDetay = new AntrenmanDetay
                    {
                        AntrenmanID = newAntrenman.AntrenmanID,
                        EgzersizID = detayDto.EgzersizID,
                        SetSayisi = detayDto.SetSayisi,
                        TekrarSayisi = detayDto.TekrarSayisi,
                        Agirlik = detayDto.Agirlik
                    };
                    _context.AntrenmanDetaylari.Add(newAntrenmanDetay);
                }

                await _context.SaveChangesAsync();

                var createdAntrenmanListDto = new AntrenmanListDto
                {
                    AntrenmanID = newAntrenman.AntrenmanID,
                    AntrenmanTarihi = newAntrenman.AntrenmanTarihi,
                    Notlar = newAntrenman.Notlar,
                    EgzersizSayisi = createAntrenmanDto.Detaylar.Count
                };

                return CreatedAtAction(nameof(GetAntrenmanDetail), new { antrenmanId = newAntrenman.AntrenmanID }, createdAntrenmanListDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CreateAntrenman Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Antrenman kaydedilirken sunucu hatası.");
            }
        }

        [HttpGet("egzersizler")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EgzersizListDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllEgzersizler()
        {
            try
            {
                var egzersizler = await _context.Egzersizler
                    .Select(e => new EgzersizListDto
                    {
                        EgzersizID = e.EgzersizID,
                        Ad = e.Ad,
                        KasGrubu = e.KasGrubu
                    })
                    .ToListAsync();

                return Ok(egzersizler);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetAllEgzersizler Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Egzersizler getirilirken sunucu hatası.");
            }
        }
    }
}