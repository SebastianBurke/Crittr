namespace Crittr.Shared.Models;

public class CaregiverAccess
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public int EnclosureId { get; set; }
    public EnclosureProfile? Enclosure { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public DateTime AccessGranted { get; set; }
}