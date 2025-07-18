// FitnessTracker.Api/FitnessTracker.Api/Dtos/AntrenmanDetailDto.cs
using System;
using System.Collections.Generic;

namespace FitnessTracker.Api.Dtos
{
    public class AntrenmanDetailDto
    {
        public int AntrenmanID { get; set; }
        public DateTime AntrenmanTarihi { get; set; }
        public string? Notlar { get; set; }
        public List<AntrenmanDetayDto> Detaylar { get; set; } = new List<AntrenmanDetayDto>();
    }

    public class AntrenmanDetayDto
    {
        public int DetayID { get; set; }
        public int EgzersizID { get; set; }
        public string EgzersizAdi { get; set; } = null!; // Egzersiz adını da DP'ye ekleyelim
        public string KasGrubu { get; set; } = null!; // Egzersizin kas grubunu da ekleyelim
        public int SetSayisi { get; set; }
        public int TekrarSayisi { get; set; }
        public decimal Agirlik { get; set; }
    }
}