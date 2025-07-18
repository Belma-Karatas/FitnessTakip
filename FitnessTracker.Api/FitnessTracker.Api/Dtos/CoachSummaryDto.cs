// FitnessTracker.Api/FitnessTracker.Api/Dtos/CoachSummaryDto.cs

namespace FitnessTracker.Api.Dtos
{
    public class CoachSummaryDto
    {
        // Null olmaması gereken alanlar için null! kullanımı yapıldı.
        public int CoachId { get; set; }
        public string CoachUserName { get; set; } = null!;
        public string CoachFullName { get; set; } = null!;
    }
}