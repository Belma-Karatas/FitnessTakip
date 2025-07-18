// FitnessTracker.Api/FitnessTracker.Api/Dtos/GoalSummaryDto.cs
using System;

namespace FitnessTracker.Api.Dtos
{
    public class GoalSummaryDto
    {
        // Null olmaması gereken alanlar için null! kullanımı yapıldı.
        public string GoalType { get; set; } = null!;
        public decimal TargetValue { get; set; }
        public decimal CurrentValue { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsAchieved { get; set; }
    }
}