// FitnessTracker.Api/FitnessTracker.Api/Dtos/ClientDashboardDto.cs
using System;
using System.Collections.Generic;

namespace FitnessTracker.Api.Dtos
{
    public class ClientDashboardDto
    {
        // Null olmaması gereken alanlar için null! kullanımı yapıldı.
        public string WelcomeName { get; set; } = null!; // Kullanıcının adı
        public int TotalWorkouts { get; set; }
        public int TotalNutritionEntries { get; set; }
        public int CompletedGoals { get; set; }
        public WorkoutSummaryDto? LastWorkout { get; set; } // Son antrenman bilgisi null olabilir
        public GoalSummaryDto? CurrentGoal { get; set; }   // Mevcut hedef bilgisi null olabilir
        public CoachSummaryDto? AssignedCoach { get; set; } // Atanan antrenör bilgisi null olabilir
    }

    // Bu iç içe tanımlanan DTO'lar için de null işaretlemeleri kontrol edildi.
    // (Yukarıda ayrı ayrı verdiklerimin aynısıdır.)
    /*
    public class WorkoutSummaryDto { ... }
    public class GoalSummaryDto { ... }
    public class CoachSummaryDto { ... }
    */
}