// FitnessTracker.Api/FitnessTracker.Api/Dtos/CreateAntrenmanDto.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Validation için

namespace FitnessTracker.Api.Dtos
{
    public class CreateAntrenmanDto
    {
        // KullaniciID zaten JWT token'dan alınacağı için burada zorunlu değil.
        // public int KullaniciID { get; set; } // Gerekirse eklenebilir

        [Required]
        public DateTime AntrenmanTarihi { get; set; }

        public string? Notlar { get; set; }

        [Required]
        [MinLength(1)] // En az bir egzersiz detayı olmalı
        public List<CreateAntrenmanDetayDto> Detaylar { get; set; } = new List<CreateAntrenmanDetayDto>();
    }

    public class CreateAntrenmanDetayDto
    {
        [Required]
        public int EgzersizID { get; set; }

        [Required]
        [Range(1, 10)] // Set sayısı için basit bir aralık
        public int SetSayisi { get; set; }

        [Required]
        [Range(1, 50)] // Tekrar sayısı için basit bir aralık
        public int TekrarSayisi { get; set; }

        [Required]
        [Range(0.1, 1000)] // Ağırlık için basit bir aralık
        public decimal Agirlik { get; set; }
    }
}