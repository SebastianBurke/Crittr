using Microsoft.AspNetCore.Identity;
using Crittr.Server.Models;
using Crittr.Shared.Models;
using static Crittr.Shared.Models.Enums.ItemType;
using static Crittr.Shared.Models.Enums.TaskPriority;

namespace Crittr.Server.Data;

public class DataSeeder
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ApplicationDbContext _dbContext;

    public DataSeeder(UserManager<AppUser> userManager, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    public async Task SeedAsync(string userIdFromArgs)
    {
        if (_dbContext.Critters.Any()) return;

        var user = await _userManager.FindByEmailAsync("demo@demo.com");
        if (user == null)
        {
            user = new AppUser
            {
                UserName = "demo@demo.com",
                Email = "demo@demo.com"
            };
            var result = await _userManager.CreateAsync(user, "Password123!");
            if (!result.Succeeded) throw new Exception("Failed to create demo user.");
        }

        var enclosure1 = new EnclosureProfile
        {
            Name = "Desert Terrarium",
            Length = 120,
            Width = 60,
            Height = 60,
            SubstrateType = "Sand/Clay mix",
            HasUVBLighting = true,
            HasHeatingElement = true,
            IdealTemperature = 32,
            IdealHumidity = 30,
            OwnerId = user.Id
        };

        var enclosure2 = new EnclosureProfile
        {
            Name = "Tropical Vivarium",
            Length = 90,
            Width = 45,
            Height = 90,
            SubstrateType = "Coconut Fiber",
            HasUVBLighting = true,
            HasHeatingElement = true,
            IdealTemperature = 28,
            IdealHumidity = 70,
            OwnerId = user.Id
        };

        _dbContext.EnclosureProfiles.AddRange(enclosure1, enclosure2);
        await _dbContext.SaveChangesAsync(); 

        var spike = new Critter
        {
            Name = "Spike",
            Species = "Bearded Dragon",
            DateAcquired = new DateTime(2024, 11, 1),
            DateOfBirth = new DateTime(2023, 5, 1),
            Sex = "Male",
            Weight = 450,
            Length = 45,
            Description = "Friendly beardie with orange coloration",
            EnclosureProfileId = enclosure1.Id
        };

        var monty = new Critter
        {
            Name = "Monty",
            Species = "Ball Python",
            DateAcquired = new DateTime(2023, 5, 1),
            DateOfBirth = new DateTime(2022, 5, 1),
            Sex = "Male",
            Weight = 1500,
            Length = 120,
            Description = "Normal morph ball python, very docile",
            EnclosureProfileId = enclosure2.Id
        };
        _dbContext.Critters.AddRange(spike, monty);
        await _dbContext.SaveChangesAsync();

        _dbContext.FeedingRecords.AddRange(
            new FeedingRecord
            {
                CritterId = spike.Id,
                FeedingDate = new DateTime(2025, 4, 29),
                FoodItem = "Crickets",
                Quantity = 12,
                ItemType = Insect,
                WasEaten = true
            },
            new FeedingRecord
            {
                CritterId = spike.Id,
                FeedingDate = new DateTime(2025, 4, 25),
                FoodItem = "Mealworms",
                Quantity = 15,
                ItemType = Insect,
                WasEaten = true
            },
            new FeedingRecord
            {
                CritterId = monty.Id,
                FeedingDate = new DateTime(2025, 4, 22),
                FoodItem = "Small Rat",
                Quantity = 1,
                ItemType = Rodent,
                WasEaten = true
            });

        _dbContext.EnvironmentalReadings.AddRange(
            new EnvironmentalReading
            {
                CritterId = spike.Id,
                ReadingDate = new DateTime(2025, 5, 4),
                Temperature = 33.5,
                Humidity = 35,
                UVBIndex = 6.7,
                IsManualReading = true,
                Source = "Manual"
            },
            new EnvironmentalReading
            {
                CritterId = monty.Id,
                ReadingDate = new DateTime(2025, 5, 4),
                Temperature = 28.0,
                Humidity = 65,
                IsManualReading = true,
                Source = "Manual"
            });

        _dbContext.SheddingRecords.AddRange(
            new SheddingRecord
            {
                CritterId = spike.Id,
                StartDate = new DateTime(2025, 4, 20),
                CompletionDate = new DateTime(2025, 4, 24),
                IsComplete = true,
                WasAssisted = false,
                Notes = "Normal shed."
            },
            new SheddingRecord
            {
                CritterId = monty.Id,
                StartDate = new DateTime(2025, 4, 10),
                CompletionDate = new DateTime(2025, 4, 15),
                IsComplete = true,
                WasAssisted = true,
                Notes = "Helped with tail shedding."
            });

        _dbContext.MeasurementRecords.AddRange(
            new MeasurementRecord
            {
                CritterId = spike.Id,
                MeasurementDate = new DateTime(2025, 4, 15),
                Weight = 460,
                Length = 46,
                Notes = "Gained some weight."
            },
            new MeasurementRecord
            {
                CritterId = monty.Id,
                MeasurementDate = new DateTime(2025, 4, 18),
                Weight = 1520,
                Length = 121,
                Notes = "Normal growth."
            });

        _dbContext.HealthScores.AddRange(
            new HealthScore
            {
                CritterId = spike.Id,
                AssessmentDate = new DateTime(2025, 4, 30),
                Score = 9,
                Notes = "Healthy and active."
            },
            new HealthScore
            {
                CritterId = monty.Id,
                AssessmentDate = new DateTime(2025, 4, 30),
                Score = 8,
                Notes = "Slight respiratory noise observed."
            });

        _dbContext.ScheduledTasks.AddRange(
            new ScheduledTask
            {
                CritterId = spike.Id,
                Title = "Clean terrarium",
                Description = "Full substrate change and decoration cleaning",
                DueDate = new DateTime(2025, 5, 8),
                IsCompleted = false,
                Priority = Medium
            },
            new ScheduledTask
            {
                CritterId = monty.Id,
                Title = "UVB Bulb Replacement",
                Description = "Replace the UVB bulb which is nearing end of its effective lifespan",
                DueDate = new DateTime(2025, 5, 12),
                IsCompleted = false,
                Priority = High
            });

        await _dbContext.SaveChangesAsync();
    }
}