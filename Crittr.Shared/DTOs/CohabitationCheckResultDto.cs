using System.ComponentModel.DataAnnotations;

namespace Crittr.Shared.DTOs;

public class CohabitationCheckResultDto
{
    public bool CanCohabit { get; set; }
    public bool HasWarnings { get; set; }
    public List<CohabitationConflictDto> Conflicts { get; set; } = new();
}

public class CohabitationConflictDto
{
    [StringLength(120)]
    public string CritterName { get; set; } = string.Empty;
    [StringLength(120)]
    public string SpeciesName { get; set; } = string.Empty;
    [StringLength(2000)]
    public string Reason { get; set; } = string.Empty;
    public bool IsHardBlock { get; set; }
}
