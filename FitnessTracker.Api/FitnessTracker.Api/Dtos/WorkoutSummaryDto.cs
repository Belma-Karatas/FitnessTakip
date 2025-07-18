// FitnessTracker.Api/FitnessTracker.Api/Dtos/WorkoutSummaryDto.cs
using System;

namespace FitnessTracker.Api.Dtos
{
    public class WorkoutSummaryDto
    {
        public DateTime WorkoutDate { get; set; }
        // Notes alanı null olabileceği için ? ile işaretlendi.
        public string? Notes { get; set; }
    }
}