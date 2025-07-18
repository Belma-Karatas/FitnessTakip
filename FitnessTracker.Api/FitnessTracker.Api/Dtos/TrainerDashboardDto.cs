// FitnessTracker.Api/FitnessTracker.Api/Dtos/TrainerDashboardDto.cs
using System;
using System.Collections.Generic;

namespace FitnessTracker.Api.Dtos
{
    public class TrainerDashboardDto
    {
        public int TotalClients { get; set; }
        public int ActiveClientsLast30Days { get; set; } // Bu mantığı gerçek veriye göre düzenlemek gerekecek.
        public int NewClientsLast7Days { get; set; }    // Bu mantığı gerçek veriye göre düzenlemek gerekecek.
        public List<ClientSummaryDto> RecentActiveClients { get; set; } = new List<ClientSummaryDto>(); // Boş liste olarak başlatıldı, null olmaması için.

        // public List<GoalReminderDto> UpcomingGoalReminders { get; set; } // Opsiyonel olarak eklenebilir
    }

    /* Opsiyonel olarak eklenebilecek GoalReminderDto örneği:
    public class GoalReminderDto
    {
        public int GoalId { get; set; }
        public string GoalDescription { get; set; } = null!;
        public DateTime DueDate { get; set; }
    }
    */
}