using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Crittr.Server.Models;
using Crittr.Shared.Models;

namespace Crittr.Server.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Critter> Critters { get; set; }
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

        modelBuilder.Entity<Critter>()
            .HasMany(r => r.FeedingRecords).WithOne(f => f.Critter).HasForeignKey(f => f.CritterId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Critter>()
            .HasMany(r => r.EnvironmentalReadings).WithOne(e => e.Critter).HasForeignKey(e => e.CritterId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Critter>()
            .HasMany(r => r.BehaviorLogs).WithOne(b => b.Critter).HasForeignKey(b => b.CritterId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Critter>()
            .HasMany(r => r.SheddingRecords).WithOne(s => s.Critter).HasForeignKey(s => s.CritterId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Critter>()
            .HasMany(r => r.MeasurementRecords).WithOne(m => m.Critter).HasForeignKey(m => m.CritterId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Critter>()
            .HasMany(r => r.ScheduledTasks).WithOne(t => t.Critter).HasForeignKey(t => t.CritterId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Critter>()
            .HasMany(r => r.HealthScores).WithOne(h => h.Critter).HasForeignKey(h => h.CritterId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Critter>()
            .HasOne(r => r.EnclosureProfile).WithMany(e => e.Critters).HasForeignKey(r => r.EnclosureProfileId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<CaregiverAccess>()
            .HasOne(c => c.Enclosure)
            .WithMany(e => e.Caregivers)
            .HasForeignKey(c => c.EnclosureId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<EnclosureProfile>()
            .HasMany(e => e.Critters)
            .WithOne(r => r.EnclosureProfile)
            .HasForeignKey(r => r.EnclosureProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
