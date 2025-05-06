using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReptileCare.Server.Models;
using ReptileCare.Shared.Models;

namespace ReptileCare.Server.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Reptile> Reptiles { get; set; }
    public DbSet<EnclosureProfile> EnclosureProfiles { get; set; }
    public DbSet<FeedingRecord> FeedingRecords { get; set; }
    public DbSet<EnvironmentalReading> EnvironmentalReadings { get; set; }
    public DbSet<BehaviorLog> BehaviorLogs { get; set; }
    public DbSet<SheddingRecord> SheddingRecords { get; set; }
    public DbSet<MeasurementRecord> MeasurementRecords { get; set; }
    public DbSet<ScheduledTask> ScheduledTasks { get; set; }
    public DbSet<HealthScore> HealthScores { get; set; }
    public DbSet<CaregiverAccess> CaregiverAccesses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Reptile>()
            .HasMany(r => r.FeedingRecords).WithOne(f => f.Reptile).HasForeignKey(f => f.ReptileId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Reptile>()
            .HasMany(r => r.EnvironmentalReadings).WithOne(e => e.Reptile).HasForeignKey(e => e.ReptileId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Reptile>()
            .HasMany(r => r.BehaviorLogs).WithOne(b => b.Reptile).HasForeignKey(b => b.ReptileId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Reptile>()
            .HasMany(r => r.SheddingRecords).WithOne(s => s.Reptile).HasForeignKey(s => s.ReptileId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Reptile>()
            .HasMany(r => r.MeasurementRecords).WithOne(m => m.Reptile).HasForeignKey(m => m.ReptileId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Reptile>()
            .HasMany(r => r.ScheduledTasks).WithOne(t => t.Reptile).HasForeignKey(t => t.ReptileId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Reptile>()
            .HasMany(r => r.HealthScores).WithOne(h => h.Reptile).HasForeignKey(h => h.ReptileId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Reptile>()
            .HasOne(r => r.EnclosureProfile).WithMany(e => e.Reptiles).HasForeignKey(r => r.EnclosureProfileId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<AppUser>()
            .HasMany(u => u.Reptiles)
            .WithOne()
            .HasForeignKey(r => r.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
