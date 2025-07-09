using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Egzersizler",
                columns: table => new
                {
                    EgzersizID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KasGrubu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Egzersizler", x => x.EgzersizID);
                });

            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    KullaniciID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciAdi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Eposta = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SifreHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Soyad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Cinsiyet = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DogumTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BoyCM = table.Column<int>(type: "int", nullable: true),
                    GuncelKiloKG = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    KocID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.KullaniciID);
                });

            migrationBuilder.CreateTable(
                name: "EgzersizFotograflari",
                columns: table => new
                {
                    FotografID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EgzersizID = table.Column<int>(type: "int", nullable: false),
                    FotoURL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SiraNo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EgzersizFotograflari", x => x.FotografID);
                    table.ForeignKey(
                        name: "FK_EgzersizFotograflari_Egzersizler_EgzersizID",
                        column: x => x.EgzersizID,
                        principalTable: "Egzersizler",
                        principalColumn: "EgzersizID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Antrenmanlar",
                columns: table => new
                {
                    AntrenmanID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciID = table.Column<int>(type: "int", nullable: false),
                    AntrenmanTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notlar = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Antrenmanlar", x => x.AntrenmanID);
                    table.ForeignKey(
                        name: "FK_Antrenmanlar_Kullanicilar_KullaniciID",
                        column: x => x.KullaniciID,
                        principalTable: "Kullanicilar",
                        principalColumn: "KullaniciID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BeslenmeGunlukleri",
                columns: table => new
                {
                    BeslenmeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciID = table.Column<int>(type: "int", nullable: false),
                    KayitTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OgunTipi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    YiyecekAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Kalori = table.Column<int>(type: "int", nullable: false),
                    Protein = table.Column<decimal>(type: "decimal(5,1)", nullable: false),
                    Karbonhidrat = table.Column<decimal>(type: "decimal(5,1)", nullable: false),
                    Yag = table.Column<decimal>(type: "decimal(5,1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeslenmeGunlukleri", x => x.BeslenmeID);
                    table.ForeignKey(
                        name: "FK_BeslenmeGunlukleri_Kullanicilar_KullaniciID",
                        column: x => x.KullaniciID,
                        principalTable: "Kullanicilar",
                        principalColumn: "KullaniciID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hedefler",
                columns: table => new
                {
                    HedefID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciID = table.Column<int>(type: "int", nullable: false),
                    HedefTipi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HedefDeger = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    BaslangicDegeri = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    BaslangicTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TamamlandiMi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hedefler", x => x.HedefID);
                    table.ForeignKey(
                        name: "FK_Hedefler_Kullanicilar_KullaniciID",
                        column: x => x.KullaniciID,
                        principalTable: "Kullanicilar",
                        principalColumn: "KullaniciID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IlerlemeTakibiKayitlari",
                columns: table => new
                {
                    IlerlemeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciID = table.Column<int>(type: "int", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KiloKG = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    YagOrani = table.Column<decimal>(type: "decimal(4,2)", nullable: true),
                    KasKutlesiKG = table.Column<decimal>(type: "decimal(5,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IlerlemeTakibiKayitlari", x => x.IlerlemeID);
                    table.ForeignKey(
                        name: "FK_IlerlemeTakibiKayitlari_Kullanicilar_KullaniciID",
                        column: x => x.KullaniciID,
                        principalTable: "Kullanicilar",
                        principalColumn: "KullaniciID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AntrenmanDetaylari",
                columns: table => new
                {
                    DetayID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AntrenmanID = table.Column<int>(type: "int", nullable: false),
                    EgzersizID = table.Column<int>(type: "int", nullable: false),
                    SetSayisi = table.Column<int>(type: "int", nullable: false),
                    TekrarSayisi = table.Column<int>(type: "int", nullable: false),
                    Agirlik = table.Column<decimal>(type: "decimal(6,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntrenmanDetaylari", x => x.DetayID);
                    table.ForeignKey(
                        name: "FK_AntrenmanDetaylari_Antrenmanlar_AntrenmanID",
                        column: x => x.AntrenmanID,
                        principalTable: "Antrenmanlar",
                        principalColumn: "AntrenmanID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AntrenmanDetaylari_Egzersizler_EgzersizID",
                        column: x => x.EgzersizID,
                        principalTable: "Egzersizler",
                        principalColumn: "EgzersizID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AntrenmanDetaylari_AntrenmanID",
                table: "AntrenmanDetaylari",
                column: "AntrenmanID");

            migrationBuilder.CreateIndex(
                name: "IX_AntrenmanDetaylari_EgzersizID",
                table: "AntrenmanDetaylari",
                column: "EgzersizID");

            migrationBuilder.CreateIndex(
                name: "IX_Antrenmanlar_KullaniciID",
                table: "Antrenmanlar",
                column: "KullaniciID");

            migrationBuilder.CreateIndex(
                name: "IX_BeslenmeGunlukleri_KullaniciID",
                table: "BeslenmeGunlukleri",
                column: "KullaniciID");

            migrationBuilder.CreateIndex(
                name: "IX_EgzersizFotograflari_EgzersizID",
                table: "EgzersizFotograflari",
                column: "EgzersizID");

            migrationBuilder.CreateIndex(
                name: "IX_Hedefler_KullaniciID",
                table: "Hedefler",
                column: "KullaniciID");

            migrationBuilder.CreateIndex(
                name: "IX_IlerlemeTakibiKayitlari_KullaniciID",
                table: "IlerlemeTakibiKayitlari",
                column: "KullaniciID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AntrenmanDetaylari");

            migrationBuilder.DropTable(
                name: "BeslenmeGunlukleri");

            migrationBuilder.DropTable(
                name: "EgzersizFotograflari");

            migrationBuilder.DropTable(
                name: "Hedefler");

            migrationBuilder.DropTable(
                name: "IlerlemeTakibiKayitlari");

            migrationBuilder.DropTable(
                name: "Antrenmanlar");

            migrationBuilder.DropTable(
                name: "Egzersizler");

            migrationBuilder.DropTable(
                name: "Kullanicilar");
        }
    }
}
