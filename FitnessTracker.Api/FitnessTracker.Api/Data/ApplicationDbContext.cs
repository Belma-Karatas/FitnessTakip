using FitnessTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Veritabanında oluşturulacak tabloları burada DbSet olarak tanımlıyoruz.
        // Entity Framework Core, bu listeye bakarak veritabanı şemasını oluşturacak.
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Egzersiz> Egzersizler { get; set; }
        public DbSet<EgzersizFotografi> EgzersizFotograflari { get; set; }
        public DbSet<Antrenman> Antrenmanlar { get; set; }
        public DbSet<AntrenmanDetay> AntrenmanDetaylari { get; set; }
        public DbSet<BeslenmeGunlugu> BeslenmeGunlukleri { get; set; }
        public DbSet<IlerlemeTakibi> IlerlemeTakibiKayitlari { get; set; } // IlerlemeTakibiKayitlari olarak isimlendirdim
        public DbSet<Hedef> Hedefler { get; set; }
    }
}