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

    public async Task SeedAsync(string userIdFromArgs)
    {
        if (_dbContext.Critters.Any()) return;

        var user = await _userManager.FindByIdAsync(userIdFromArgs);
        if (user == null)
            throw new InvalidOperationException("Demo user was not found; cannot seed sample data.");

        // One enclosure of every type
        var terrarium = new EnclosureProfile
        {
            Name = "Sandy Desert Terrarium",
            EnclosureType = EnclosureType.Terrarium,
            Length = 48, Width = 24, Height = 24,
            SubstrateType = "Sand/clay mix",
            HasUVBLighting = true, HasHeatingElement = true,
            OwnerId = user.Id
        };
        var aquarium = new EnclosureProfile
        {
            Name = "Planted Aquarium",
            EnclosureType = EnclosureType.Aquarium,
            Length = 36, Width = 18, Height = 18,
            SubstrateType = "Fine gravel",
            HasUVBLighting = false, HasHeatingElement = false,
            OwnerId = user.Id
        };
        var paludarium = new EnclosureProfile
        {
            Name = "Tropical Paludarium",
            EnclosureType = EnclosureType.Paludarium,
            Length = 36, Width = 18, Height = 36,
            SubstrateType = "ABG mix",
            HasUVBLighting = true, HasHeatingElement = true,
            OwnerId = user.Id
        };
        var vivarium = new EnclosureProfile
        {
            Name = "Bioactive Vivarium",
            EnclosureType = EnclosureType.Vivarium,
            Length = 24, Width = 18, Height = 36,
            SubstrateType = "Coconut fiber",
            HasUVBLighting = true, HasHeatingElement = true,
            OwnerId = user.Id
        };
        var insectarium = new EnclosureProfile
        {
            Name = "Tarantula Insectarium",
            EnclosureType = EnclosureType.Insectarium,
            Length = 12, Width = 12, Height = 18,
            SubstrateType = "Coco peat",
            HasUVBLighting = false, HasHeatingElement = false,
            OwnerId = user.Id
        };
        var aviary = new EnclosureProfile
        {
            Name = "Bird Aviary",
            EnclosureType = EnclosureType.Aviary,
            Length = 36, Width = 24, Height = 48,
            SubstrateType = "Bird-safe paper liner",
            HasUVBLighting = false, HasHeatingElement = false,
            OwnerId = user.Id
        };
        var cage = new EnclosureProfile
        {
            Name = "Rat Cage",
            EnclosureType = EnclosureType.Cage,
            Length = 24, Width = 16, Height = 24,
            SubstrateType = "Fleece / Carefresh",
            HasUVBLighting = false, HasHeatingElement = false,
            OwnerId = user.Id
        };
        var bin = new EnclosureProfile
        {
            Name = "Corn Snake Bin",
            EnclosureType = EnclosureType.Bin,
            Length = 36, Width = 18, Height = 12,
            SubstrateType = "Paper towel",
            HasUVBLighting = false, HasHeatingElement = true,
            OwnerId = user.Id
        };
        var rackSystem = new EnclosureProfile
        {
            Name = "Ball Python Rack",
            EnclosureType = EnclosureType.RackSystem,
            Length = 36, Width = 18, Height = 6,
            SubstrateType = "Paper towel",
            HasUVBLighting = false, HasHeatingElement = true,
            OwnerId = user.Id
        };
        var freeRoam = new EnclosureProfile
        {
            Name = "Rabbit Free-Roam Room",
            EnclosureType = EnclosureType.FreeRoamRoom,
            Length = 12, Width = 10, Height = 8,
            SubstrateType = null,
            HasUVBLighting = false, HasHeatingElement = false,
            OwnerId = user.Id
        };
        var tank = new EnclosureProfile
        {
            Name = "Goldfish Tank",
            EnclosureType = EnclosureType.Tank,
            Length = 24, Width = 12, Height = 16,
            SubstrateType = "Gravel",
            HasUVBLighting = false, HasHeatingElement = false,
            OwnerId = user.Id
        };
        var other = new EnclosureProfile
        {
            Name = "Axolotl Custom Build",
            EnclosureType = EnclosureType.Other,
            Length = 36, Width = 18, Height = 18,
            SubstrateType = "Fine sand",
            HasUVBLighting = false, HasHeatingElement = false,
            OwnerId = user.Id
        };

        _dbContext.EnclosureProfiles.AddRange(
            terrarium, aquarium, paludarium, vivarium,
            insectarium, aviary, cage, bin,
            rackSystem, freeRoam, tank, other);
        await _dbContext.SaveChangesAsync();

        // One critter per enclosure, species matched to type
        var beardie = new Critter
        {
            Name = "Draco",
            Species = "Bearded Dragon",
            IconUrl = "img/critters/default.svg",
            SpeciesType = SpeciesType.Reptile,
            DateAcquired = new DateTime(2024, 3, 10),
            DateOfBirth = new DateTime(2023, 9, 1),
            Sex = "Male",
            Weight = 380,
            Length = 42,
            Description = "Friendly beardie, loves basking",
            EnclosureProfileId = terrarium.Id,
            UserId = user.Id
        };
        var betta = new Critter
        {
            Name = "Blaze",
            Species = "Betta Fish",
            IconUrl = "img/critters/betta-splendens.svg",
            SpeciesType = SpeciesType.Fish,
            DateAcquired = new DateTime(2025, 1, 5),
            Sex = "Male",
            Description = "Halfmoon betta, deep blue colouration",
            EnclosureProfileId = aquarium.Id,
            UserId = user.Id
        };
        var frog = new Critter
        {
            Name = "Leaf",
            Species = "Red-Eyed Tree Frog",
            IconUrl = "img/critters/default.svg",
            SpeciesType = SpeciesType.Amphibian,
            DateAcquired = new DateTime(2024, 7, 20),
            Sex = "Female",
            Description = "Active at night, bright red eyes",
            EnclosureProfileId = paludarium.Id,
            UserId = user.Id
        };
        var gecko = new Critter
        {
            Name = "Ficus",
            Species = "Crested Gecko",
            IconUrl = "img/critters/default.svg",
            SpeciesType = SpeciesType.Reptile,
            DateAcquired = new DateTime(2024, 5, 15),
            DateOfBirth = new DateTime(2024, 2, 1),
            Sex = "Female",
            Weight = 42,
            Length = 18,
            Description = "Dalmatian morph, very calm",
            EnclosureProfileId = vivarium.Id,
            UserId = user.Id
        };
        var tarantula = new Critter
        {
            Name = "Duchess",
            Species = "Chilean Rose Tarantula",
            IconUrl = "img/critters/default.svg",
            SpeciesType = SpeciesType.Invertebrate,
            DateAcquired = new DateTime(2023, 11, 3),
            Sex = "Female",
            Description = "Very calm, rarely flicks hairs",
            EnclosureProfileId = insectarium.Id,
            UserId = user.Id
        };
        var cockatiel = new Critter
        {
            Name = "Sunny",
            Species = "Cockatiel",
            IconUrl = "img/critters/nymphicus-hollandicus.svg",
            SpeciesType = SpeciesType.Bird,
            DateAcquired = new DateTime(2024, 2, 14),
            DateOfBirth = new DateTime(2023, 10, 1),
            Sex = "Male",
            Description = "Whistles constantly, loves millet",
            EnclosureProfileId = aviary.Id,
            UserId = user.Id
        };
        var rat = new Critter
        {
            Name = "Biscuit",
            Species = "Fancy Rat",
            IconUrl = "img/critters/default.svg",
            SpeciesType = SpeciesType.Mammal,
            DateAcquired = new DateTime(2025, 2, 1),
            DateOfBirth = new DateTime(2024, 12, 15),
            Sex = "Female",
            Weight = 280,
            Description = "Hooded rat, very social",
            EnclosureProfileId = cage.Id,
            UserId = user.Id
        };
        var cornSnake = new Critter
        {
            Name = "Candy",
            Species = "Corn Snake",
            IconUrl = "img/critters/default.svg",
            SpeciesType = SpeciesType.Reptile,
            DateAcquired = new DateTime(2024, 8, 10),
            DateOfBirth = new DateTime(2024, 4, 1),
            Sex = "Female",
            Weight = 200,
            Length = 60,
            Description = "Amelanistic morph, great feeder",
            EnclosureProfileId = bin.Id,
            UserId = user.Id
        };
        var ballPython = new Critter
        {
            Name = "Monty",
            Species = "Ball Python",
            IconUrl = "img/critters/python-regius.svg",
            SpeciesType = SpeciesType.Reptile,
            DateAcquired = new DateTime(2023, 5, 1),
            DateOfBirth = new DateTime(2022, 5, 1),
            Sex = "Male",
            Weight = 1500,
            Length = 120,
            Description = "Normal morph, very docile",
            EnclosureProfileId = rackSystem.Id,
            UserId = user.Id
        };
        var rabbit = new Critter
        {
            Name = "Clover",
            Species = "Holland Lop Rabbit",
            IconUrl = "img/critters/default.svg",
            SpeciesType = SpeciesType.Mammal,
            DateAcquired = new DateTime(2024, 10, 5),
            DateOfBirth = new DateTime(2024, 7, 20),
            Sex = "Female",
            Weight = 1800,
            Description = "Grey lop ears, litter trained",
            EnclosureProfileId = freeRoam.Id,
            UserId = user.Id
        };
        var goldfish = new Critter
        {
            Name = "Bubbles",
            Species = "Common Goldfish",
            IconUrl = "img/critters/default.svg",
            SpeciesType = SpeciesType.Fish,
            DateAcquired = new DateTime(2025, 3, 1),
            Description = "Orange and white fantail",
            EnclosureProfileId = tank.Id,
            UserId = user.Id
        };
        var axolotl = new Critter
        {
            Name = "Noodle",
            Species = "Axolotl",
            IconUrl = "img/critters/default.svg",
            SpeciesType = SpeciesType.Amphibian,
            DateAcquired = new DateTime(2024, 12, 1),
            Sex = "Male",
            Description = "Leucistic, responds to feeding time",
            EnclosureProfileId = other.Id,
            UserId = user.Id
        };

        _dbContext.Critters.AddRange(
            beardie, betta, frog, gecko, tarantula, cockatiel,
            rat, cornSnake, ballPython, rabbit, goldfish, axolotl);
        await _dbContext.SaveChangesAsync();

        // Feeding records with 2026 dates — designed to produce a visible spread of condition badges:
        // Draco (beardie, freq=2d): fed today → Thriving 😊
        // Blaze (betta, freq=1d): fed 2 days ago → Good 🙂
        // Leaf (tree frog, freq=3d): fed 2 days ago → Thriving 😊
        // Ficus (crested gecko, freq=2d): fed 6 days ago → Fair 😐
        // Duchess (rose tarantula, freq=14d, natural faster): fed 23 days ago → Thriving 😊 (ratio < 4, no penalty)
        // Sunny (cockatiel, freq=1d): fed today → Thriving 😊
        // Biscuit (rat, freq=1d): fed 4 days ago → Attention ⚠️ (3× overdue)
        // Candy (corn snake, freq=7d): fed 14 days ago → Good 🙂 (ratio=1)
        // Monty (ball python, freq=10d, natural faster): fed 9 days ago → Thriving 😊
        // Clover (rabbit, freq=1d): fed 2 days ago → Good 🙂
        // Bubbles (goldfish, freq=1d): fed 3 days ago → Fair 😐
        // Noodle (axolotl, freq=2d): fed 2 days ago → Thriving 😊
        _dbContext.FeedingRecords.AddRange(
            // Draco — Thriving
            new FeedingRecord { CritterId = beardie.Id,    FeedingDate = new DateTime(2026, 4, 24), FoodItem = "Dubia roaches", Quantity = 10, ItemType = Insect, WasEaten = true },
            new FeedingRecord { CritterId = beardie.Id,    FeedingDate = new DateTime(2026, 4, 22), FoodItem = "Dubia roaches", Quantity = 10, ItemType = Insect, WasEaten = true },
            new FeedingRecord { CritterId = beardie.Id,    FeedingDate = new DateTime(2026, 4, 20), FoodItem = "Collard greens", Quantity = 1, ItemType = Other,  WasEaten = true },
            // Blaze — Good
            new FeedingRecord { CritterId = betta.Id,      FeedingDate = new DateTime(2026, 4, 22), FoodItem = "Betta pellets", Quantity = 1, ItemType = Other, WasEaten = true },
            new FeedingRecord { CritterId = betta.Id,      FeedingDate = new DateTime(2026, 4, 21), FoodItem = "Betta pellets", Quantity = 1, ItemType = Other, WasEaten = true },
            new FeedingRecord { CritterId = betta.Id,      FeedingDate = new DateTime(2026, 4, 20), FoodItem = "Bloodworms",    Quantity = 1, ItemType = Other, WasEaten = true },
            // Leaf — Thriving
            new FeedingRecord { CritterId = frog.Id,       FeedingDate = new DateTime(2026, 4, 22), FoodItem = "Crickets",   Quantity = 5, ItemType = Insect, WasEaten = true },
            new FeedingRecord { CritterId = frog.Id,       FeedingDate = new DateTime(2026, 4, 19), FoodItem = "Crickets",   Quantity = 5, ItemType = Insect, WasEaten = true },
            new FeedingRecord { CritterId = frog.Id,       FeedingDate = new DateTime(2026, 4, 16), FoodItem = "Mealworms",  Quantity = 3, ItemType = Insect, WasEaten = true },
            // Ficus — Fair (6 days since last feed, freq=2)
            new FeedingRecord { CritterId = gecko.Id,      FeedingDate = new DateTime(2026, 4, 18), FoodItem = "CGD mix",    Quantity = 1, ItemType = Other, WasEaten = true },
            new FeedingRecord { CritterId = gecko.Id,      FeedingDate = new DateTime(2026, 4, 16), FoodItem = "Crickets",   Quantity = 4, ItemType = Insect, WasEaten = true },
            new FeedingRecord { CritterId = gecko.Id,      FeedingDate = new DateTime(2026, 4, 14), FoodItem = "CGD mix",    Quantity = 1, ItemType = Other, WasEaten = true },
            // Duchess — Thriving (natural faster, 23 days is within acceptable range)
            new FeedingRecord { CritterId = tarantula.Id,  FeedingDate = new DateTime(2026, 4, 1),  FoodItem = "Crickets",   Quantity = 2, ItemType = Insect, WasEaten = true },
            new FeedingRecord { CritterId = tarantula.Id,  FeedingDate = new DateTime(2026, 3, 18), FoodItem = "Dubia roaches", Quantity = 2, ItemType = Insect, WasEaten = true },
            // Sunny — Thriving
            new FeedingRecord { CritterId = cockatiel.Id,  FeedingDate = new DateTime(2026, 4, 24), FoodItem = "Pellets + fresh veg", Quantity = 1, ItemType = Other, WasEaten = true },
            new FeedingRecord { CritterId = cockatiel.Id,  FeedingDate = new DateTime(2026, 4, 23), FoodItem = "Millet spray",         Quantity = 1, ItemType = Other, WasEaten = true },
            new FeedingRecord { CritterId = cockatiel.Id,  FeedingDate = new DateTime(2026, 4, 22), FoodItem = "Pellets + fresh veg", Quantity = 1, ItemType = Other, WasEaten = true },
            // Biscuit — Attention (4 days overdue for a daily animal)
            new FeedingRecord { CritterId = rat.Id,        FeedingDate = new DateTime(2026, 4, 20), FoodItem = "Lab blocks",   Quantity = 1, ItemType = Other, WasEaten = true },
            new FeedingRecord { CritterId = rat.Id,        FeedingDate = new DateTime(2026, 4, 19), FoodItem = "Lab blocks",   Quantity = 1, ItemType = Other, WasEaten = true },
            new FeedingRecord { CritterId = rat.Id,        FeedingDate = new DateTime(2026, 4, 17), FoodItem = "Fresh vegetables", Quantity = 1, ItemType = Other, WasEaten = true },
            // Candy — Good (14 days since last feed, freq=7; ratio=1, -20 pts)
            new FeedingRecord { CritterId = cornSnake.Id,  FeedingDate = new DateTime(2026, 4, 10), FoodItem = "Pinky mouse",  Quantity = 1, ItemType = Rodent, WasEaten = true },
            new FeedingRecord { CritterId = cornSnake.Id,  FeedingDate = new DateTime(2026, 4, 3),  FoodItem = "Pinky mouse",  Quantity = 1, ItemType = Rodent, WasEaten = true },
            new FeedingRecord { CritterId = cornSnake.Id,  FeedingDate = new DateTime(2026, 3, 27), FoodItem = "Pinky mouse",  Quantity = 1, ItemType = Rodent, WasEaten = true },
            // Monty — Thriving (9 days, freq=10; not yet overdue)
            new FeedingRecord { CritterId = ballPython.Id, FeedingDate = new DateTime(2026, 4, 15), FoodItem = "Small rat",    Quantity = 1, ItemType = Rodent, WasEaten = true },
            new FeedingRecord { CritterId = ballPython.Id, FeedingDate = new DateTime(2026, 4, 5),  FoodItem = "Small rat",    Quantity = 1, ItemType = Rodent, WasEaten = true },
            new FeedingRecord { CritterId = ballPython.Id, FeedingDate = new DateTime(2026, 3, 26), FoodItem = "Small rat",    Quantity = 1, ItemType = Rodent, WasEaten = true },
            // Clover — Good (2 days, freq=1; ratio=1, -20 pts)
            new FeedingRecord { CritterId = rabbit.Id,     FeedingDate = new DateTime(2026, 4, 22), FoodItem = "Timothy hay + pellets", Quantity = 1, ItemType = Other, WasEaten = true },
            new FeedingRecord { CritterId = rabbit.Id,     FeedingDate = new DateTime(2026, 4, 21), FoodItem = "Timothy hay + pellets", Quantity = 1, ItemType = Other, WasEaten = true },
            new FeedingRecord { CritterId = rabbit.Id,     FeedingDate = new DateTime(2026, 4, 20), FoodItem = "Bell pepper + greens",  Quantity = 1, ItemType = Other, WasEaten = true },
            // Bubbles — Fair (3 days, freq=1; ratio=2, -40 pts)
            new FeedingRecord { CritterId = goldfish.Id,   FeedingDate = new DateTime(2026, 4, 21), FoodItem = "Goldfish pellets", Quantity = 1, ItemType = Other, WasEaten = true },
            new FeedingRecord { CritterId = goldfish.Id,   FeedingDate = new DateTime(2026, 4, 20), FoodItem = "Bloodworms",       Quantity = 1, ItemType = Other, WasEaten = true },
            new FeedingRecord { CritterId = goldfish.Id,   FeedingDate = new DateTime(2026, 4, 19), FoodItem = "Goldfish pellets", Quantity = 1, ItemType = Other, WasEaten = true },
            // Noodle — Thriving (2 days, freq=2; exactly on schedule)
            new FeedingRecord { CritterId = axolotl.Id,    FeedingDate = new DateTime(2026, 4, 22), FoodItem = "Nightcrawlers", Quantity = 2, ItemType = Other, WasEaten = true },
            new FeedingRecord { CritterId = axolotl.Id,    FeedingDate = new DateTime(2026, 4, 20), FoodItem = "Brine shrimp",  Quantity = 1, ItemType = Other, WasEaten = true },
            new FeedingRecord { CritterId = axolotl.Id,    FeedingDate = new DateTime(2026, 4, 18), FoodItem = "Nightcrawlers", Quantity = 2, ItemType = Other, WasEaten = true }
        );

        await _dbContext.SaveChangesAsync();
    }
}
