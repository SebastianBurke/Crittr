using Crittr.Shared.DTOs;
using Crittr.Shared.Models.Enums;

namespace Crittr.Server.Services.Interfaces;

public interface IHealthAnalyticsEngine
{
    CritterCondition ComputeCondition(CritterDto dto, SpeciesCareProfile? careProfile);
}
