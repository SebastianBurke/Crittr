namespace Crittr.Shared.DTOs;

public class CohabitationCheckResultDto
{
    public bool CanCohabit { get; set; }
    public bool HasWarnings { get; set; }
    public List<CohabitationConflictDto> Conflicts { get; set; } = new();
}

public class CohabitationConflictDto
{
    public string CritterName { get; set; } = string.Empty;   // the pet's name (e.g. "Ripley")
    public string SpeciesName { get; set; } = string.Empty;   // the species (e.g. "Ball Python")
    public string Reason { get; set; } = string.Empty;
    public bool IsHardBlock { get; set; }
}
