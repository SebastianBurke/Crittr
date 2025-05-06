namespace ReptileCare.Shared.DTOs;

public class CaregiverAccessDto
{
    public int Id { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public int EnclosureId { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public DateTime AccessGranted { get; set; }
}