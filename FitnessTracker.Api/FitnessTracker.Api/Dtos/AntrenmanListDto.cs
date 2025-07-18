// FitnessTracker.Api/FitnessTracker.Api/Dtos/AntrenmanListDto.cs
using System;
using System.Collections.Generic;

namespace FitnessTracker.Api.Dtos
{
    public class AntrenmanListDto
    {
        public int AntrenmanID { get; set; }
        public DateTime AntrenmanTarihi { get; set; }
        public string? Notlar { get; set; }
        public int EgzersizSayisi { get; set; } // Bu antrenmanda kaç farklı egzersiz yapıldığı bilgisi
    }
}