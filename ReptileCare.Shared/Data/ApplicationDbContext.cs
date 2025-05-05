using Microsoft.EntityFrameworkCore;
using ReptileCare.Shared.Models; // Or whatever namespace your models are in

namespace ReptileCare.Shared.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Reptile> Reptiles { get; set; }
    // Add other DbSets as needed
}