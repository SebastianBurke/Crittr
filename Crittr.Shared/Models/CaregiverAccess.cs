using System.ComponentModel.DataAnnotations;

namespace Crittr.Shared.Models;

public class CaregiverAccess
{
    public int Id { get; set; }
    [StringLength(450)]
    public string UserId { get; set; } = string.Empty;
    [StringLength(254)]
    public string UserEmail { get; set; } = string.Empty;
    public int EnclosureId { get; set; }
    public EnclosureProfile? Enclosure { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public DateTime AccessGranted { get; set; }
}
