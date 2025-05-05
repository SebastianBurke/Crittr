using Microsoft.EntityFrameworkCore;
using ReptileCare.Shared.Models;

namespace ReptileCare.Shared.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

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
            .HasMany(r => r.FeedingRecords).WithOne(f => f.Reptile).HasForeignKey(f => f.ReptileId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Reptile>()
            .HasMany(r => r.EnvironmentalReadings).WithOne(e => e.Reptile).HasForeignKey(e => e.ReptileId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Reptile>()
            .HasMany(r => r.BehaviorLogs).WithOne(b => b.Reptile).HasForeignKey(b => b.ReptileId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Reptile>()
            .HasMany(r => r.SheddingRecords).WithOne(s => s.Reptile).HasForeignKey(s => s.ReptileId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Reptile>()
            .HasMany(r => r.MeasurementRecords).WithOne(m => m.Reptile).HasForeignKey(m => m.ReptileId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Reptile>()
            .HasMany(r => r.ScheduledTasks).WithOne(t => t.Reptile).HasForeignKey(t => t.ReptileId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Reptile>()
            .HasMany(r => r.HealthScores).WithOne(h => h.Reptile).HasForeignKey(h => h.ReptileId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Reptile>()
            .HasOne(r => r.EnclosureProfile).WithMany(e => e.Reptiles).HasForeignKey(r => r.EnclosureProfileId).OnDelete(DeleteBehavior.SetNull);

        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EnclosureProfile>().HasData(
            new EnclosureProfile { Id = 1, Name = "Desert Terrarium", Length = 120, Width = 60, Height = 60, SubstrateType = "Sand/Clay mix", HasUVBLighting = true, HasHeatingElement = true, IdealTemperature = 32, IdealHumidity = 30 },
            new EnclosureProfile { Id = 2, Name = "Tropical Vivarium", Length = 90, Width = 45, Height = 90, SubstrateType = "Coconut Fiber", HasUVBLighting = true, HasHeatingElement = true, IdealTemperature = 28, IdealHumidity = 70 }
        );

        modelBuilder.Entity<Reptile>().HasData(
            new Reptile { Id = 1, Name = "Spike", Species = "Bearded Dragon", DateAcquired = new DateTime(2024, 11, 1), DateOfBirth = new DateTime(2023, 5, 1), Sex = "Male", Weight = 450, Length = 45, Description = "Friendly beardie with orange coloration", EnclosureProfileId = 1 },
            new Reptile { Id = 2, Name = "Monty", Species = "Ball Python", DateAcquired = new DateTime(2023, 5, 1), DateOfBirth = new DateTime(2022, 5, 1), Sex = "Male", Weight = 1500, Length = 120, Description = "Normal morph ball python, very docile", EnclosureProfileId = 2 }
        );

        modelBuilder.Entity<FeedingRecord>().HasData(
            new FeedingRecord { Id = 1, ReptileId = 1, FeedingDate = new DateTime(2025, 4, 29), FoodItem = "Crickets", Quantity = 12, ItemType = Models.Enums.ItemType.Insect, WasEaten = true },
            new FeedingRecord { Id = 2, ReptileId = 1, FeedingDate = new DateTime(2025, 4, 25), FoodItem = "Mealworms", Quantity = 15, ItemType = Models.Enums.ItemType.Insect, WasEaten = true },
            new FeedingRecord { Id = 3, ReptileId = 2, FeedingDate = new DateTime(2025, 4, 22), FoodItem = "Small Rat", Quantity = 1, ItemType = Models.Enums.ItemType.Rodent, WasEaten = true }
        );

        modelBuilder.Entity<EnvironmentalReading>().HasData(
            new EnvironmentalReading { Id = 1, ReptileId = 1, ReadingDate = new DateTime(2025, 5, 4), Temperature = 33.5, Humidity = 35, UVBIndex = 6.7, IsManualReading = true, Source = "Manual" },
            new EnvironmentalReading { Id = 2, ReptileId = 2, ReadingDate = new DateTime(2025, 5, 4), Temperature = 28.0, Humidity = 65, IsManualReading = true, Source = "Manual" }
        );

        modelBuilder.Entity<SheddingRecord>().HasData(
            new SheddingRecord { Id = 1, ReptileId = 1, StartDate = new DateTime(2025, 4, 20), CompletionDate = new DateTime(2025, 4, 24), IsComplete = true, WasAssisted = false, Notes = "Normal shed." },
            new SheddingRecord { Id = 2, ReptileId = 2, StartDate = new DateTime(2025, 4, 10), CompletionDate = new DateTime(2025, 4, 15), IsComplete = true, WasAssisted = true, Notes = "Helped with tail shedding." }
        );

        modelBuilder.Entity<MeasurementRecord>().HasData(
            new MeasurementRecord { Id = 1, ReptileId = 1, MeasurementDate = new DateTime(2025, 4, 15), Weight = 460, Length = 46, Notes = "Gained some weight." },
            new MeasurementRecord { Id = 2, ReptileId = 2, MeasurementDate = new DateTime(2025, 4, 18), Weight = 1520, Length = 121, Notes = "Normal growth." }
        );

        modelBuilder.Entity<HealthScore>().HasData(
            new HealthScore { Id = 1, ReptileId = 1, AssessmentDate = new DateTime(2025, 4, 30), Score = 9, Notes = "Healthy and active." },
            new HealthScore { Id = 2, ReptileId = 2, AssessmentDate = new DateTime(2025, 4, 30), Score = 8, Notes = "Slight respiratory noise observed." }
        );

        modelBuilder.Entity<ScheduledTask>().HasData(
            new ScheduledTask { Id = 1, ReptileId = 1, Title = "Clean terrarium", Description = "Full substrate change and decoration cleaning", DueDate = new DateTime(2025, 5, 8), IsCompleted = false, Priority = Models.Enums.TaskPriority.Medium },
            new ScheduledTask { Id = 2, ReptileId = 2, Title = "UVB Bulb Replacement", Description = "Replace the UVB bulb which is nearing end of its effective lifespan", DueDate = new DateTime(2025, 5, 12), IsCompleted = false, Priority = Models.Enums.TaskPriority.High }
        );
    }
}
