using Crittr.Server.Services.Interfaces;
using Crittr.Shared.DTOs;
using Crittr.Shared.Models.Enums;

namespace Crittr.Server.Services;

public class HealthAnalyticsEngine : IHealthAnalyticsEngine
{
    public CritterCondition ComputeCondition(CritterDto dto, SpeciesCareProfile? care)
    {
        if (care == null)
            return CritterCondition.Unknown;

        // No feeding history means we don't have enough data to assess condition
        if (!dto.LastFeedingDate.HasValue)
            return CritterCondition.Unknown;

        int score = 100;
        var today = DateTime.UtcNow;

        // ── Feeding signal (highest weight) ──────────────────────────────────
        {
            var daysSinceFeed = (today - dto.LastFeedingDate.Value).TotalDays;
            var overdueDays = daysSinceFeed - care.FeedingFrequencyDays;

            if (overdueDays > 0)
            {
                // Natural fasters (ball pythons, tarantulas, etc.) get a much softer penalty
                double ratio = overdueDays / (double)care.FeedingFrequencyDays;
                if (care.NaturalFastingNormal)
                {
                    if (ratio >= 6) score -= 40;
                    else if (ratio >= 4) score -= 20;
                    // mild overdue = no penalty for natural fasters
                }
                else
                {
                    if (ratio >= 3) score -= 60;
                    else if (ratio >= 2) score -= 40;
                    else if (ratio >= 1) score -= 20;
                    else score -= 10;
                }
            }
        }

        // ── Shedding signal (only for species that shed) ──────────────────────
        if (care.SheddingIntervalDays.HasValue && dto.LastSheddingDate.HasValue)
        {
            var daysSinceShed = (today - dto.LastSheddingDate.Value).TotalDays;
            // Allow 50% grace period beyond the expected interval before docking
            var overdueThreshold = care.SheddingIntervalDays.Value * 1.5;
            if (daysSinceShed > overdueThreshold)
            {
                double ratio = (daysSinceShed - overdueThreshold) / care.SheddingIntervalDays.Value;
                score -= ratio >= 1 ? 20 : 10;
            }
        }

        // ── Pending tasks signal (low weight) ────────────────────────────────
        score -= dto.PendingTasksCount switch
        {
            >= 5 => 20,
            >= 3 => 10,
            >= 1 => 5,
            _ => 0
        };

        return score switch
        {
            >= 90 => CritterCondition.Thriving,
            >= 75 => CritterCondition.Good,
            >= 55 => CritterCondition.Fair,
            >= 30 => CritterCondition.Attention,
            _ => CritterCondition.Critical
        };
    }
}
