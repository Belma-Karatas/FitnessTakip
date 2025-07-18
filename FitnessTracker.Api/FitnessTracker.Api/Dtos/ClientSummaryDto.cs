// FitnessTracker.Api/FitnessTracker.Api/Dtos/ClientSummaryDto.cs
using System;

namespace FitnessTracker.Api.Dtos
{
    public class ClientSummaryDto
    {
        public int ClientId { get; set; }
        // Null olmaması gereken alanlar için null! kullanımı yapıldı.
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public DateTime LastWorkoutDate { get; set; }
    }
}