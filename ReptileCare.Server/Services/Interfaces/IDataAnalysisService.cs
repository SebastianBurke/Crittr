namespace ReptileCare.Server.Services.Interfaces;

public interface IDataAnalysisService
{
    Task<Dictionary<string, double>> GetFeedingStatisticsAsync(int reptileId, int days);
    Task<Dictionary<string, double>> GetEnvironmentalStatisticsAsync(int reptileId, int days);
    Task<Dictionary<string, int>> GetBehaviorDistributionAsync(int reptileId, int days);
    Task<Dictionary<DateTime, double>> GetWeightTrendDataAsync(int reptileId, int months);
    Task<Dictionary<string, int>> GetHealthIndicatorsAsync(int reptileId);
    Task<Dictionary<string, double>> GetSpeciesComparisonAsync(string species, int reptileId);
}
