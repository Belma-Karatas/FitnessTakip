// FitnessTracker.Api/FitnessTracker.Api/Controllers/DashboardController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessTracker.Api.Data;
using FitnessTracker.Api.Dtos;
using FitnessTracker.Api.Models;
using Microsoft.AspNetCore.Authorization; // Authorize attribute'ü için
using System.Security.Claims; // ClaimsPrincipal için
using System.Linq; // LINQ metodları için
using System; // DateTime için
using System.Collections.Generic; // List için
using System.Threading.Tasks; // Task için

namespace FitnessTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Bu controller'daki tüm endpoint'ler için kimlik doğrulama gereklidir.
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            // Null kontrolü ve ArgumentNullException ile daha güvenli bir başlangıç.
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Giriş yapmış kullanıcının rolüne göre dashboard verilerini getirir.
        /// </summary>
        /// <returns>Dashboard verileri.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))] // Hem trainer hem client için generic response
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Örneğin, danışan antrenörsüz ise
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDashboardData()
        {
            // ClaimTypes.NameIdentifier genellikle kullanıcının ID'sini tutar.
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userRoleClaim = User.FindFirst(ClaimTypes.Role);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
            {
                // Kullanıcı ID'si alınamazsa veya geçersizse yetkisiz döndür
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            if (userRoleClaim == null)
            {
                // Kullanıcı rolü alınamazsa
                return Unauthorized("Kullanıcı rolü bilgisi eksik.");
            }

            var userRole = userRoleClaim.Value;

            try
            {
                if (userRole == "Antrenor")
                {
                    var trainerDashboardData = await GetTrainerDashboardData(currentUserId);
                    // Antrenör dashboard verisi null dönerse (örneğin hiç danışanı yoksa)
                    if (trainerDashboardData == null)
                    {
                        // Boş bir DTO döndürmek daha kullanıcı dostu olabilir.
                        // Ancak NotFound dönmek de bir seçenektir. Biz boş DTO döndürelim.
                        return Ok(new TrainerDashboardDto
                        {
                            TotalClients = 0,
                            ActiveClientsLast30Days = 0,
                            NewClientsLast7Days = 0,
                            RecentActiveClients = new List<ClientSummaryDto>()
                        });
                    }
                    return Ok(trainerDashboardData);
                }
                else if (userRole == "Danisan")
                {
                    var clientDashboardData = await GetClientDashboardData(currentUserId);
                    // Eğer danışanın verileri gelmezse (kullanıcı bulunamazsa) NotFound döndür.
                    if (clientDashboardData == null)
                    {
                        return NotFound("Danışan dashboard verileri bulunamadı.");
                    }
                    return Ok(clientDashboardData);
                }
                else
                {
                    // Tanımlanmayan bir rol gelirse veya yetkisi yoksa
                    return Forbid("Bu role sahip kullanıcılar için dashboard bilgisi bulunmuyor.");
                }
            }
            catch (Exception ex)
            {
                // Hata loglama burada yapılmalı (örneğin Serilog, NLog vb.)
                // Debug amaçlı basit loglama: CS0168 hatasını gidermek için ex kullanılır hale getirildi.
                Console.WriteLine($"DashboardController Error: {ex.Message}");
                // Loglama yapılması önerilir (ex.Message, ex.StackTrace)
                return StatusCode(StatusCodes.Status500InternalServerError, "Dashboard verileri getirilirken bir sunucu hatası oluştu.");
            }
        }

        /// <summary>
        /// Antrenör dashboard'u için gerekli verileri hazırlar.
        /// </summary>
        private async Task<TrainerDashboardDto?> GetTrainerDashboardData(int coachId)
        {
            // Antrenöre ait tüm danışanları çek.
            // 'OlusturulmaTarihi' yerine daha anlamlı bir 'SonAktiviteTarihi' veya benzeri bir alan olsa daha iyi olurdu.
            // Şimdilik 'OlusturulmaTarihi' ile basit bir örnek yapalım.
            var clients = await _context.Kullanicilar
                .Where(u => u.KocID == coachId)
                .ToListAsync();

            if (clients == null || !clients.Any()) // Eğer hiç danışan yoksa
            {
                // Boş bir DTO döndürelim ki frontend boş liste görebilsin.
                return new TrainerDashboardDto
                {
                    TotalClients = 0,
                    ActiveClientsLast30Days = 0,
                    NewClientsLast7Days = 0,
                    RecentActiveClients = new List<ClientSummaryDto>()
                };
            }

            var today = DateTime.UtcNow;

            // Her bir danışan için son antrenman tarihini bulalım.
            var recentActiveClients = new List<ClientSummaryDto>();
            var clientTasks = clients.Select(async client =>
            {
                var lastWorkoutDate = await _context.Antrenmanlar
                    .Where(a => a.KullaniciID == client.KullaniciID)
                    .OrderByDescending(a => a.AntrenmanTarihi)
                    .Select(a => a.AntrenmanTarihi)
                    .FirstOrDefaultAsync();

                return new ClientSummaryDto
                {
                    ClientId = client.KullaniciID,
                    UserName = client.KullaniciAdi,
                    FullName = $"{client.Ad ?? ""} {client.Soyad ?? ""}".Trim(),
                    // Eğer son antrenman yoksa (DateTime.MinValue), bu danışanı sıralamada sona atacağız.
                    LastWorkoutDate = lastWorkoutDate == DateTime.MinValue ? DateTime.MinValue : lastWorkoutDate
                };
            }).ToList();

            var allClientSummaries = await Task.WhenAll(clientTasks);

            // Aktif danışanları ve yeni kayıtlıları hesapla.
            // Bu hesaplamalar için daha anlamlı kriterler (örn. son aktivite tarihi) gereklidir.
            // Şimdilik sadece örnek amaçlı oluşturulma tarihlerini kullanıyoruz.
            var activeClientsLast30Days = clients.Count(c => c.OlusturulmaTarihi.AddDays(30) > today);
            var newClientsLast7Days = clients.Count(c => c.OlusturulmaTarihi.AddDays(7) > today);

            // RecentActiveClients'ı filtrelenmiş ve sıralanmış haliyle alalım.
            // Sadece antrenman yapmış olanları ve tarihe göre sıralayalım.
            var filteredAndSortedRecentClients = allClientSummaries
                .Where(c => c.LastWorkoutDate > DateTime.MinValue) // Antrenman yapmış olanları al
                .OrderByDescending(c => c.LastWorkoutDate)
                .Take(5)
                .ToList();


            var trainerDashboard = new TrainerDashboardDto
            {
                TotalClients = clients.Count,
                ActiveClientsLast30Days = activeClientsLast30Days,
                NewClientsLast7Days = newClientsLast7Days,
                RecentActiveClients = filteredAndSortedRecentClients
            };

            return trainerDashboard;
        }

        /// <summary>
        /// Danışan dashboard'u için gerekli verileri hazırlar.
        /// </summary>
        private async Task<ClientDashboardDto?> GetClientDashboardData(int userId)
        {
            // Kullanıcıyı, antrenmanlarını ve hedeflerini çekelim.
            var user = await _context.Kullanicilar
                .Include(u => u.Antrenmanlar) // Antrenmanları dahil et
                .Include(u => u.Hedefler)    // Hedefleri dahil et
                .FirstOrDefaultAsync(u => u.KullaniciID == userId);

            if (user == null) return null; // Kullanıcı bulunamadı

            // Son antrenman bilgisini al
            var lastWorkout = user.Antrenmanlar
                .OrderByDescending(w => w.AntrenmanTarihi)
                .Select(w => new WorkoutSummaryDto
                {
                    WorkoutDate = w.AntrenmanTarihi,
                    Notes = w.Notlar != null && w.Notlar.Length > 70 ? w.Notlar.Substring(0, 70) + "..." : w.Notlar
                })
                .FirstOrDefault(); // En son antrenmanı getir

            // Aktif hedef bilgisini al (örneğin ilk gelen veya bitiş tarihi en yakın olan)
            var currentGoal = user.Hedefler
                .Where(g => !g.TamamlandiMi)
                .OrderBy(g => g.BitisTarihi)
                .Select(g => new GoalSummaryDto
                {
                    GoalType = g.HedefTipi,
                    TargetValue = g.HedefDeger,
                    // Mevcut değer için: ProgressTracking tablosundan çekilmeli veya hedef tipine göre hesaplanmalı.
                    // Şimdilik sadece başlangıç değerini alıyoruz, bu kısım geliştirilmeli.
                    CurrentValue = g.BaslangicDegeri,
                    EndDate = g.BitisTarihi,
                    IsAchieved = g.TamamlandiMi
                })
                .FirstOrDefault(); // İlk aktif hedefi getir

            // Antrenör bilgisini çek
            CoachSummaryDto? coachInfo = null;
            if (user.KocID.HasValue)
            {
                // Kullanıcının KocID'si varsa, bu ID'ye sahip kullanıcıyı (antrenör olarak) çek.
                var coach = await _context.Kullanicilar.FindAsync(user.KocID.Value);
                if (coach != null)
                {
                    coachInfo = new CoachSummaryDto
                    {
                        CoachId = coach.KullaniciID,
                        CoachUserName = coach.KullaniciAdi,
                        CoachFullName = $"{coach.Ad ?? ""} {coach.Soyad ?? ""}".Trim()
                    };
                }
            }

            // Genel sayılar için sorgular
            // Bu sorgular, verimlilik için tek bir sorguda birleştirilebilir veya ViewModel'lar kullanılabilir.
            // Ancak mevcut haliyle de çalışır.
            int totalWorkouts = await _context.Antrenmanlar.CountAsync(w => w.KullaniciID == userId);
            int totalNutritionEntries = await _context.BeslenmeGunlukleri.CountAsync(n => n.KullaniciID == userId);
            int completedGoals = await _context.Hedefler.CountAsync(g => g.KullaniciID == userId && g.TamamlandiMi);

            var clientDashboard = new ClientDashboardDto
            {
                // Ad yoksa kullanıcı adını kullan.
                WelcomeName = !string.IsNullOrEmpty(user.Ad) ? user.Ad : user.KullaniciAdi,
                TotalWorkouts = totalWorkouts,
                TotalNutritionEntries = totalNutritionEntries,
                CompletedGoals = completedGoals,
                LastWorkout = lastWorkout,
                CurrentGoal = currentGoal,
                AssignedCoach = coachInfo
            };

            return clientDashboard;
        }
    }
}