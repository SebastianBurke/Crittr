using Microsoft.AspNetCore.Identity;
using Crittr.Server.Models;
using Crittr.Shared.Models;
using Crittr.Shared.Models.Enums;
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

    /// <summary>Corrects EnclosureType for existing rows that were seeded before the field was populated.</summary>
    public async Task FixEnclosureTypesAsync()
    {
        var fixes = new Dictionary<string, EnclosureType>(StringComparer.OrdinalIgnoreCase)
        {
            ["vivarium"]    = EnclosureType.Vivarium,
            ["aquarium"]    = EnclosureType.Aquarium,
            ["paludarium"]  = EnclosureType.Paludarium,
            ["insectarium"] = EnclosureType.Insectarium,
            ["aviary"]      = EnclosureType.Aviary,
            ["cage"]        = EnclosureType.Cage,
            ["rack"]        = EnclosureType.RackSystem,
            ["bin"]         = EnclosureType.Bin,
            ["tank"]        = EnclosureType.Tank,
        };

        var enclosures = _dbContext.EnclosureProfiles
            .Where(e => e.EnclosureType == EnclosureType.Terrarium)
            .ToList();

        bool changed = false;
        foreach (var enc in enclosures)
        {
            foreach (var (keyword, type) in fixes)
            {
                if (enc.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                {
                    enc.EnclosureType = type;
                    changed = true;
                    break;
                }
            }
        }

        if (changed) await _dbContext.SaveChangesAsync();
    }

    public async Task SeedAsync(string userIdFromArgs)
    {
        if (_dbContext.Critters.Any()) return;

        var user = await _userManager.FindByIdAsync(userIdFromArgs);
        if (user == null)
            throw new InvalidOperationException(
                "Demo user from startup seed was not found; cannot attach sample critters.");

        var enclosure1 = new EnclosureProfile
        {
            Name = "Desert Terrarium",
            EnclosureType = EnclosureType.Terrarium,
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
            EnclosureType = EnclosureType.Vivarium,
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
            Name = "Billy",
            Species = "Leopard Gecko",
            IconUrl = "img/critters/eublepharis-macularius.svg",
            DateAcquired = new DateTime(2024, 11, 1),
            DateOfBirth = new DateTime(2023, 5, 1),
            Sex = "Male",
            Weight = 450,
            Length = 45,
            Description = "Friendly leo with yellow coloration",
            EnclosureProfileId = enclosure1.Id,
            UserId = user.Id
        };

        var monty = new Critter
        {
            Name = "Monty",
            Species = "Ball Python",
            IconUrl = "img/critters/python-regius.svg",
            DateAcquired = new DateTime(2023, 5, 1),
            DateOfBirth = new DateTime(2022, 5, 1),
            Sex = "Male",
            Weight = 1500,
            Length = 120,
            Description = "Normal morph ball python, very docile",
            EnclosureProfileId = enclosure2.Id,
            UserId = user.Id
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