namespace Crittr.Server.Services.Interfaces;

public interface IDataAnalysisService
{
    Task<Dictionary<string, double>> GetFeedingStatisticsAsync(int critterId, int days);
    Task<Dictionary<string, double>> GetEnvironmentalStatisticsAsync(int critterId, int days);
    Task<Dictionary<string, int>> GetBehaviorDistributionAsync(int critterId, int days);
    Task<Dictionary<DateTime, double>> GetWeightTrendDataAsync(int critterId, int months);
    Task<Dictionary<string, int>> GetHealthIndicatorsAsync(int critterId);
    Task<Dictionary<string, double>> GetSpeciesComparisonAsync(string species, int critterId);
}
