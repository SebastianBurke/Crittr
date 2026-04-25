using System.ComponentModel.DataAnnotations;

namespace Crittr.Shared.DTOs;

public class CaregiverAccessDto
{
    public int Id { get; set; }
    [StringLength(254)]
    public string UserEmail { get; set; } = string.Empty;
    public int EnclosureId { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public DateTime AccessGranted { get; set; }
}
