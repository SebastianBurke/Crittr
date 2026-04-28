using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Crittr.Server.Models;
using Crittr.Shared.Models;
using Crittr.Shared.Models.Enums;
using static Crittr.Shared.Models.Enums.ItemType;

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
        if (_dbContext.Critters.Any(c => c.UserId == userIdFromArgs)) return;

        var user = await _userManager.FindByIdAsync(userIdFromArgs);
        if (user == null)
            throw new InvalidOperationException("Demo user was not found; cannot seed sample data.");

        await SeedLineupAsync(user.Id);
    }

    /// <summary>
    /// Wipes the demo user's enclosures, critters, and all critter-child records, then re-seeds the lineup.
    /// Critter children (feedings, environmental, behavior, shedding, measurement, scheduled tasks, health scores)
    /// cascade on critter deletion (configured in <see cref="ApplicationDbContext"/>); EnclosureProfile -> Critter
    /// is SetNull, so critters must be deleted before enclosures.
    /// </summary>
    public async Task ResetAsync(string userId)
    {
        var critterIds = await _dbContext.Critters
            .Where(c => c.UserId == userId)
            .Select(c => c.Id)
            .ToListAsync();

        if (critterIds.Count > 0)
        {
            // CaregiverAccess is keyed off enclosure, not critter — handled with enclosure delete below.
            _dbContext.Critters.RemoveRange(_dbContext.Critters.Where(c => critterIds.Contains(c.Id)));
        }

        _dbContext.EnclosureProfiles.RemoveRange(
            _dbContext.EnclosureProfiles.Where(e => e.OwnerId == userId));

        await _dbContext.SaveChangesAsync();

        await SeedLineupAsync(userId);
    }

    private async Task SeedLineupAsync(string ownerId)
    {
        // ----------------- Enclosures (one per active EnclosureType) -----------------
        var terrarium = new EnclosureProfile
        {
            Name = "Royal Standoff",
            EnclosureType = EnclosureType.Terrarium,
            Length = 48, Width = 24, Height = 24,
            SubstrateType = "Cypress mulch",
            HasUVBLighting = false, HasHeatingElement = true,
            OwnerId = ownerId
        };
        var aquarium = new EnclosureProfile
        {
            Name = "Predator's Tank",
            EnclosureType = EnclosureType.Aquarium,
            Length = 48, Width = 18, Height = 20,
            SubstrateType = "Fine gravel",
            HasUVBLighting = false, HasHeatingElement = true,
            OwnerId = ownerId
        };
        var paludarium = new EnclosureProfile
        {
            Name = "Tropical Trio",
            EnclosureType = EnclosureType.Paludarium,
            Length = 36, Width = 18, Height = 36,
            SubstrateType = "ABG mix + sphagnum",
            HasUVBLighting = true, HasHeatingElement = true,
            OwnerId = ownerId
        };
        var vivarium = new EnclosureProfile
        {
            Name = "Dart Frog Colony",
            EnclosureType = EnclosureType.Vivarium,
            Length = 36, Width = 18, Height = 24,
            SubstrateType = "ABG mix",
            HasUVBLighting = true, HasHeatingElement = false,
            OwnerId = ownerId
        };
        var insectarium = new EnclosureProfile
        {
            Name = "Tarantula Lair",
            EnclosureType = EnclosureType.Insectarium,
            Length = 12, Width = 12, Height = 18,
            SubstrateType = "Coco peat",
            HasUVBLighting = false, HasHeatingElement = false,
            OwnerId = ownerId
        };
        var aviary = new EnclosureProfile
        {
            Name = "Budgie Flock",
            EnclosureType = EnclosureType.Aviary,
            Length = 48, Width = 30, Height = 60,
            SubstrateType = "Bird-safe paper liner",
            HasUVBLighting = true, HasHeatingElement = false,
            OwnerId = ownerId
        };
        var cage = new EnclosureProfile
        {
            Name = "Rat Pack",
            EnclosureType = EnclosureType.Cage,
            Length = 32, Width = 21, Height = 36,
            SubstrateType = "Fleece / Carefresh",
            HasUVBLighting = false, HasHeatingElement = false,
            OwnerId = ownerId
        };
        var bin = new EnclosureProfile
        {
            Name = "Quarantine Bin",
            EnclosureType = EnclosureType.Bin,
            Length = 36, Width = 18, Height = 12,
            SubstrateType = "Paper towel",
            HasUVBLighting = false, HasHeatingElement = true,
            OwnerId = ownerId
        };
        var rackSystem = new EnclosureProfile
        {
            Name = "Python Rack #4",
            EnclosureType = EnclosureType.RackSystem,
            Length = 36, Width = 18, Height = 6,
            SubstrateType = "Aspen shavings",
            HasUVBLighting = false, HasHeatingElement = true,
            OwnerId = ownerId
        };
        var freeRoam = new EnclosureProfile
        {
            Name = "Bunny Lounge",
            EnclosureType = EnclosureType.FreeRoamRoom,
            Length = 12, Width = 10, Height = 8,
            SubstrateType = null,
            HasUVBLighting = false, HasHeatingElement = false,
            OwnerId = ownerId
        };
        var tank = new EnclosureProfile
        {
            Name = "Goldfish Pond",
            EnclosureType = EnclosureType.Tank,
            Length = 30, Width = 14, Height = 18,
            SubstrateType = "Smooth pebbles",
            HasUVBLighting = false, HasHeatingElement = false,
            OwnerId = ownerId
        };

        _dbContext.EnclosureProfiles.AddRange(
            terrarium, aquarium, paludarium, vivarium,
            insectarium, aviary, cage, bin,
            rackSystem, freeRoam, tank);
        await _dbContext.SaveChangesAsync();

        // ----------------- Critters (curated cohab demo) -----------------
        // Terrarium — HARD BLOCK conflict: two SoloOnly Ball Pythons cohabiting
        var apollo = NewCritter("Apollo", "Ball Python", SpeciesType.Reptile, terrarium.Id, ownerId,
            sex: "Male", weight: 1600, length: 130, iconUrl: "img/critters/python-regius.svg",
            description: "Pastel morph, established 2023");
        var athena = NewCritter("Athena", "Ball Python", SpeciesType.Reptile, terrarium.Id, ownerId,
            sex: "Female", weight: 2100, length: 145, iconUrl: "img/critters/python-regius.svg",
            description: "Spider morph, gravid in spring 2025");

        // Aquarium — WARNING: Angelfish (predatory) in a small-fish community
        var spike = NewCritter("Spike", "Angelfish", SpeciesType.Fish, aquarium.Id, ownerId,
            sex: "Male", description: "Veil-tail, dominant in the planted scape");
        var tetras = Enumerable.Range(1, 6).Select(i => NewCritter(
            $"Neon {i}", "Neon Tetra", SpeciesType.Fish, aquarium.Id, ownerId,
            description: "Schooling tetra in the upper third of the column")).ToList();

        // Paludarium — GroupsOk harmony
        var toads = Enumerable.Range(1, 3).Select(i => NewCritter(
            $"Toad {i}", "Fire-bellied Toad", SpeciesType.Amphibian, paludarium.Id, ownerId,
            description: "Vivid orange belly, splash zone resident")).ToList();

        // Vivarium — GroupsOk same-morph dart frog colony
        var darts = Enumerable.Range(1, 4).Select(i => NewCritter(
            $"Tinc {i}", "Poison Dart Frog", SpeciesType.Amphibian, vivarium.Id, ownerId,
            description: "Azureus morph, captive-bred")).ToList();

        // Insectarium — SoloOnly correct
        var velvet = NewCritter("Velvet", "Chilean Rose Tarantula", SpeciesType.Invertebrate, insectarium.Id, ownerId,
            sex: "Female", description: "Mature female, calm temperament");

        // Aviary — GroupsOk harmony
        var budgies = Enumerable.Range(1, 4).Select(i => NewCritter(
            $"Budgie {i}", "Budgerigar", SpeciesType.Bird, aviary.Id, ownerId,
            description: "Mixed-sex flock, blue and green morphs")).ToList();

        // Cage — Community female-only rat colony
        var rats = Enumerable.Range(1, 3).Select(i => NewCritter(
            $"Rat {i}", "Fancy Rat", SpeciesType.Mammal, cage.Id, ownerId,
            sex: "Female", weight: 290, description: "Hooded, bonded sisters")).ToList();

        // Bin — SoloOnly correct
        var ginger = NewCritter("Ginger", "Corn Snake", SpeciesType.Reptile, bin.Id, ownerId,
            sex: "Female", weight: 220, length: 70,
            description: "Amelanistic morph, reliable feeder");

        // Rack System — SoloOnly correct
        var monty = NewCritter("Monty", "Ball Python", SpeciesType.Reptile, rackSystem.Id, ownerId,
            sex: "Male", weight: 1500, length: 120, iconUrl: "img/critters/python-regius.svg",
            description: "Normal morph, very docile");

        // FreeRoamRoom — PairsOk bonded female pair (Holland Lop morph)
        var clover = NewCritter("Clover", "Rabbit", SpeciesType.Mammal, freeRoam.Id, ownerId,
            sex: "Female", weight: 1800,
            description: "Holland Lop, grey, litter trained");
        var sage = NewCritter("Sage", "Rabbit", SpeciesType.Mammal, freeRoam.Id, ownerId,
            sex: "Female", weight: 1750,
            description: "Holland Lop, white with spots, bonded with Clover");

        // Tank — Community goldfish pair
        var bubbles = NewCritter("Bubbles", "Common Goldfish", SpeciesType.Fish, tank.Id, ownerId,
            description: "Orange and white fantail");
        var pebble = NewCritter("Pebble", "Common Goldfish", SpeciesType.Fish, tank.Id, ownerId,
            description: "Calico telescope-eye");

        var allCritters = new List<Critter>
        {
            apollo, athena, spike, velvet, ginger, monty, clover, sage, bubbles, pebble
        };
        allCritters.AddRange(tetras);
        allCritters.AddRange(toads);
        allCritters.AddRange(darts);
        allCritters.AddRange(budgies);
        allCritters.AddRange(rats);

        _dbContext.Critters.AddRange(allCritters);
        await _dbContext.SaveChangesAsync();

        // ----------------- Feeding records -----------------
        // Tuned against species FeedingFrequencyDays so the dashboard surfaces a mix of condition badges.
        // "Today" baseline: 2026-04-28.
        var feedings = new List<FeedingRecord>();

        // Apollo (ball python, freq=10d, natural faster) — Thriving (8 days ago)
        feedings.AddRange(SnakeFeed(apollo.Id, new DateTime(2026, 4, 20)));
        feedings.AddRange(SnakeFeed(apollo.Id, new DateTime(2026, 4, 10)));
        feedings.AddRange(SnakeFeed(apollo.Id, new DateTime(2026, 3, 31)));

        // Athena (ball python) — Good (12 days ago, slightly overdue)
        feedings.AddRange(SnakeFeed(athena.Id, new DateTime(2026, 4, 16)));
        feedings.AddRange(SnakeFeed(athena.Id, new DateTime(2026, 4, 5)));
        feedings.AddRange(SnakeFeed(athena.Id, new DateTime(2026, 3, 25)));

        // Spike (angelfish, freq=1d) — Good (2 days ago)
        feedings.AddRange(FishFlake(spike.Id, new DateTime(2026, 4, 26)));
        feedings.AddRange(FishFlake(spike.Id, new DateTime(2026, 4, 25)));
        feedings.AddRange(FishFlake(spike.Id, new DateTime(2026, 4, 24)));

        // Tetras — Thriving (fed today)
        foreach (var t in tetras)
        {
            feedings.AddRange(FishFlake(t.Id, new DateTime(2026, 4, 28)));
            feedings.AddRange(FishFlake(t.Id, new DateTime(2026, 4, 27)));
        }

        // Toads (freq=2d) — Thriving (today)
        foreach (var t in toads)
        {
            feedings.Add(InsectFeed(t.Id, new DateTime(2026, 4, 28), "Crickets", 4));
            feedings.Add(InsectFeed(t.Id, new DateTime(2026, 4, 26), "Crickets", 4));
        }

        // Dart frogs (freq=2d) — Good (3 days ago)
        foreach (var d in darts)
        {
            feedings.Add(InsectFeed(d.Id, new DateTime(2026, 4, 25), "Springtails", 1));
            feedings.Add(InsectFeed(d.Id, new DateTime(2026, 4, 23), "Fruit flies", 1));
        }

        // Velvet (rose tarantula, freq=14d, natural faster) — Thriving (10 days ago)
        feedings.Add(InsectFeed(velvet.Id, new DateTime(2026, 4, 18), "Dubia roach", 1));
        feedings.Add(InsectFeed(velvet.Id, new DateTime(2026, 4, 4),  "Cricket",     1));

        // Budgies (freq=1d) — Thriving (today)
        foreach (var b in budgies)
        {
            feedings.Add(SeedFeed(b.Id, new DateTime(2026, 4, 28), "Pellets + millet"));
            feedings.Add(SeedFeed(b.Id, new DateTime(2026, 4, 27), "Pellets + greens"));
        }

        // Rats (freq=1d) — Attention (4 days overdue)
        foreach (var r in rats)
        {
            feedings.Add(GenericFeed(r.Id, new DateTime(2026, 4, 24), "Lab blocks"));
            feedings.Add(GenericFeed(r.Id, new DateTime(2026, 4, 23), "Lab blocks"));
            feedings.Add(GenericFeed(r.Id, new DateTime(2026, 4, 21), "Fresh vegetables"));
        }

        // Ginger (corn snake, freq=7d) — Good (10 days ago)
        feedings.AddRange(SnakeFeed(ginger.Id, new DateTime(2026, 4, 18), "Pinky mouse"));
        feedings.AddRange(SnakeFeed(ginger.Id, new DateTime(2026, 4, 11), "Pinky mouse"));
        feedings.AddRange(SnakeFeed(ginger.Id, new DateTime(2026, 4, 4),  "Pinky mouse"));

        // Monty (ball python) — Thriving (6 days ago)
        feedings.AddRange(SnakeFeed(monty.Id, new DateTime(2026, 4, 22)));
        feedings.AddRange(SnakeFeed(monty.Id, new DateTime(2026, 4, 12)));
        feedings.AddRange(SnakeFeed(monty.Id, new DateTime(2026, 4, 2)));

        // Clover & Sage (rabbit, freq=1d) — Good
        foreach (var rab in new[] { clover, sage })
        {
            feedings.Add(GenericFeed(rab.Id, new DateTime(2026, 4, 27), "Timothy hay + pellets"));
            feedings.Add(GenericFeed(rab.Id, new DateTime(2026, 4, 26), "Timothy hay + pellets"));
            feedings.Add(GenericFeed(rab.Id, new DateTime(2026, 4, 25), "Bell pepper + greens"));
        }

        // Bubbles & Pebble (goldfish, freq=1d) — Fair (3 days ago)
        foreach (var fish in new[] { bubbles, pebble })
        {
            feedings.AddRange(FishFlake(fish.Id, new DateTime(2026, 4, 25)));
            feedings.AddRange(FishFlake(fish.Id, new DateTime(2026, 4, 24)));
            feedings.AddRange(FishFlake(fish.Id, new DateTime(2026, 4, 23)));
        }

        _dbContext.FeedingRecords.AddRange(feedings);
        await _dbContext.SaveChangesAsync();
    }

    private static Critter NewCritter(
        string name,
        string species,
        SpeciesType speciesType,
        int enclosureId,
        string ownerId,
        string? sex = null,
        double? weight = null,
        double? length = null,
        string? description = null,
        string iconUrl = "img/critters/default.svg")
        => new()
        {
            Name = name,
            Species = species,
            SpeciesType = speciesType,
            IconUrl = iconUrl,
            DateAcquired = new DateTime(2025, 1, 1),
            Sex = sex,
            Weight = weight,
            Length = length,
            Description = description,
            EnclosureProfileId = enclosureId,
            UserId = ownerId
        };

    private static IEnumerable<FeedingRecord> SnakeFeed(int critterId, DateTime date, string item = "Small rat")
        => new[]
        {
            new FeedingRecord
            {
                CritterId = critterId, FeedingDate = date,
                FoodItem = item, Quantity = 1, ItemType = Rodent, WasEaten = true
            }
        };

    private static IEnumerable<FeedingRecord> FishFlake(int critterId, DateTime date)
        => new[]
        {
            new FeedingRecord
            {
                CritterId = critterId, FeedingDate = date,
                FoodItem = "Flake food", Quantity = 1, ItemType = Other, WasEaten = true
            }
        };

    private static FeedingRecord InsectFeed(int critterId, DateTime date, string food, int qty)
        => new()
        {
            CritterId = critterId, FeedingDate = date,
            FoodItem = food, Quantity = qty, ItemType = Insect, WasEaten = true
        };

    private static FeedingRecord SeedFeed(int critterId, DateTime date, string food)
        => new()
        {
            CritterId = critterId, FeedingDate = date,
            FoodItem = food, Quantity = 1, ItemType = Other, WasEaten = true
        };

    private static FeedingRecord GenericFeed(int critterId, DateTime date, string food)
        => new()
        {
            CritterId = critterId, FeedingDate = date,
            FoodItem = food, Quantity = 1, ItemType = Other, WasEaten = true
        };
}
