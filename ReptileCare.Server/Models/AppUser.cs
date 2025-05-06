using Microsoft.AspNetCore.Identity;
using ReptileCare.Shared.Models;

namespace ReptileCare.Server.Models;


public class AppUser : IdentityUser
{
    public ICollection<Reptile> Reptiles { get; set; } = new List<Reptile>();
}
