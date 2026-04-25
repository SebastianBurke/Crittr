using Crittr.Shared.DTOs;
using Crittr.Shared.Models.Enums;

namespace Crittr.Shared.Utilities;

/// <summary>
/// Static cohabitation profile lookup keyed by species CommonName.
/// Lives in Shared so the WASM client can run checks locally without any server call.
/// </summary>
public static class CohabitationCatalog
{
    private static readonly Dictionary<string, CohabitationProfile> _profiles =
        new(StringComparer.OrdinalIgnoreCase)
    {
        // ── Reptiles ──────────────────────────────────────────────────────────
        ["Leopard Gecko"]            = new() { SocialNeeds = SocialStructure.PairsOk,   IsTerritorial = true,  IsPredatory = false, GenderRule = GenderPairingRule.OneMalePerEnclosure, MaleMaleIsLethal = true,  CohabNote = "Males will fight aggressively and can kill each other — females-only groups, or a single male with females" },
        ["Ball Python"]              = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = false, IsPredatory = true },
        ["Bearded Dragon"]           = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = true,  IsPredatory = false, CohabNote = "Will bully and stress cage mates" },
        ["Crested Gecko"]            = new() { SocialNeeds = SocialStructure.PairsOk,   IsTerritorial = true,  IsPredatory = false, GenderRule = GenderPairingRule.OneMalePerEnclosure, MaleMaleIsLethal = true,  CohabNote = "Males fight seriously — one male per enclosure maximum" },
        ["Corn Snake"]               = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = false, IsPredatory = true },
        ["Blue-tongued Skink"]       = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = true,  IsPredatory = false },
        ["Veiled Chameleon"]         = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = true,  IsPredatory = false, CohabNote = "Extreme stress from sight of another chameleon" },
        ["Panther Chameleon"]        = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = true,  IsPredatory = false, CohabNote = "Strictly solitary" },
        ["Jackson's Chameleon"]      = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = true,  IsPredatory = false, CohabNote = "Strictly solitary" },
        ["Green Iguana"]             = new() { SocialNeeds = SocialStructure.PairsOk,   IsTerritorial = true,  IsPredatory = false, GenderRule = GenderPairingRule.OneMalePerEnclosure, CohabNote = "Male iguanas are highly territorial — one male per enclosure" },
        ["Chinese Water Dragon"]     = new() { SocialNeeds = SocialStructure.PairsOk,   IsTerritorial = true,  IsPredatory = false, GenderRule = GenderPairingRule.OneMalePerEnclosure, CohabNote = "Males fight and can cause serious injury — one male per enclosure" },
        ["Uromastyx"]                = new() { SocialNeeds = SocialStructure.PairsOk,   IsTerritorial = true,  IsPredatory = false, GenderRule = GenderPairingRule.OneMalePerEnclosure, CohabNote = "Males compete and fight — one male per enclosure" },
        ["Argentine Black and White Tegu"] = new() { SocialNeeds = SocialStructure.SoloOnly, IsTerritorial = true, IsPredatory = true },
        ["Ackie Monitor"]            = new() { SocialNeeds = SocialStructure.PairsOk,   IsTerritorial = true,  IsPredatory = true,  CohabNote = "Pairs possible with adequate space" },
        ["Savannah Monitor"]         = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = true,  IsPredatory = true },
        ["Gargoyle Gecko"]           = new() { SocialNeeds = SocialStructure.PairsOk,   IsTerritorial = true,  IsPredatory = false, GenderRule = GenderPairingRule.OneMalePerEnclosure, MaleMaleIsLethal = true,  CohabNote = "Males fight — one male per enclosure" },
        ["African Fat-tailed Gecko"] = new() { SocialNeeds = SocialStructure.PairsOk,   IsTerritorial = true,  IsPredatory = false, GenderRule = GenderPairingRule.OneMalePerEnclosure, MaleMaleIsLethal = true,  CohabNote = "Males fight — females-only groups" },
        ["Giant Day Gecko"]          = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = true,  IsPredatory = false, GenderRule = GenderPairingRule.OneMalePerEnclosure, MaleMaleIsLethal = true,  CohabNote = "Strictly one per enclosure; males fight to the death" },
        ["Tokay Gecko"]              = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = true,  IsPredatory = false, GenderRule = GenderPairingRule.OneMalePerEnclosure, MaleMaleIsLethal = true,  CohabNote = "Males fight; highly territorial — one per enclosure" },
        ["Kenyan Sand Boa"]          = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = false, IsPredatory = true },
        ["Hognose Snake"]            = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = false, IsPredatory = true },
        ["Boa Constrictor"]          = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = false, IsPredatory = true },
        ["Carpet Python"]            = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = false, IsPredatory = true },
        ["Green Tree Python"]        = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = false, IsPredatory = true },
        ["Blood Python"]             = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = false, IsPredatory = true },
        ["King Snake"]               = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = false, IsPredatory = true,  IncompatibleWith = ["Milk Snake", "Corn Snake", "Ball Python"], CohabNote = "Ophiophagous — will eat other snakes" },
        ["Milk Snake"]               = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = false, IsPredatory = true,  CohabNote = "Ophiophagous — will eat other snakes" },
        ["Russian Tortoise"]         = new() { SocialNeeds = SocialStructure.PairsOk,   IsTerritorial = true,  IsPredatory = false, GenderRule = GenderPairingRule.OneMalePerEnclosure, CohabNote = "Males ram and harass each other and females relentlessly — one male per enclosure" },
        ["Hermann's Tortoise"]       = new() { SocialNeeds = SocialStructure.PairsOk,   IsTerritorial = true,  IsPredatory = false, GenderRule = GenderPairingRule.OneMalePerEnclosure, CohabNote = "Males compete and harass females; one male per enclosure" },
        ["Sulcata Tortoise"]         = new() { SocialNeeds = SocialStructure.PairsOk,   IsTerritorial = true,  IsPredatory = false, GenderRule = GenderPairingRule.OneMalePerEnclosure, CohabNote = "Males fight and ram each other — one male per enclosure" },
        ["Red-eared Slider"]         = new() { SocialNeeds = SocialStructure.GroupsOk,  IsTerritorial = false, IsPredatory = false, GenderRule = GenderPairingRule.OneMalePerEnclosure, CohabNote = "Males harass females and fight other males — one male per group" },
        ["Box Turtle"]               = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = false, IsPredatory = false, CohabNote = "Largely solitary in the wild" },

        // ── Amphibians ────────────────────────────────────────────────────────
        ["Axolotl"]                  = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = false, IsPredatory = true,  CohabNote = "Cannibalistic; will bite off limbs of enclosure mates" },
        ["Pacman Frog"]              = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = false, IsPredatory = true,  CohabNote = "Ambush predator; will eat anything that moves" },
        ["White's Tree Frog"]        = new() { SocialNeeds = SocialStructure.GroupsOk,  IsTerritorial = false, IsPredatory = false, CohabNote = "Groups of same species do well together" },
        ["Red-eyed Tree Frog"]       = new() { SocialNeeds = SocialStructure.GroupsOk,  IsTerritorial = false, IsPredatory = false, CohabNote = "Groups of same species" },
        ["Poison Dart Frog"]         = new() { SocialNeeds = SocialStructure.GroupsOk,  IsTerritorial = false, IsPredatory = false, CohabNote = "Same species/morph recommended; cross-morph risks hybridization" },
        ["African Bullfrog"]         = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = false, IsPredatory = true,  CohabNote = "Highly aggressive; will eat other frogs" },
        ["Fire-bellied Toad"]        = new() { SocialNeeds = SocialStructure.GroupsOk,  IsTerritorial = false, IsPredatory = false, CohabNote = "Can be kept in groups; skin secretions toxic to other species" },
        ["Tomato Frog"]              = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = false, IsPredatory = false, CohabNote = "Secretes skin irritant; largely solitary" },
        ["Tiger Salamander"]         = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = false, IsPredatory = true,  CohabNote = "Cannibalistic with smaller individuals" },
        ["Fire Salamander"]          = new() { SocialNeeds = SocialStructure.PairsOk,   IsTerritorial = false, IsPredatory = false },

        // ── Birds ─────────────────────────────────────────────────────────────
        ["Cockatiel"]                = new() { SocialNeeds = SocialStructure.GroupsOk,  IsTerritorial = false, IsPredatory = false, GenderRule = GenderPairingRule.OneMalePerEnclosure, CohabNote = "Two males can become territorial and fight, especially in breeding season — a male/female pair or all-female group is recommended" },
        ["Budgerigar"]               = new() { SocialNeeds = SocialStructure.GroupsOk,  IsTerritorial = false, IsPredatory = false, CohabNote = "Highly social; pairs or groups" },
        ["African Grey Parrot"]      = new() { SocialNeeds = SocialStructure.PairsOk,   IsTerritorial = true,  IsPredatory = false, CohabNote = "Can be territorial; large birds can injure smaller ones" },
        ["Blue and Gold Macaw"]      = new() { SocialNeeds = SocialStructure.PairsOk,   IsTerritorial = true,  IsPredatory = false, CohabNote = "Large; can injure smaller birds" },
        ["Green-cheeked Conure"]     = new() { SocialNeeds = SocialStructure.GroupsOk,  IsTerritorial = false, IsPredatory = false, CohabNote = "Social; pairs or small groups of same species" },
        ["Sun Conure"]               = new() { SocialNeeds = SocialStructure.PairsOk,   IsTerritorial = false, IsPredatory = false, CohabNote = "Pairs work well" },
        ["Lovebird"]                 = new() { SocialNeeds = SocialStructure.PairsOk,   IsTerritorial = true,  IsPredatory = false, GenderRule = GenderPairingRule.OneMalePerEnclosure, CohabNote = "Same-sex male pairs can become aggressive — bonded M/F pairs preferred" },
        ["Caique"]                   = new() { SocialNeeds = SocialStructure.PairsOk,   IsTerritorial = true,  IsPredatory = false, GenderRule = GenderPairingRule.OneMalePerEnclosure, CohabNote = "Males can fight — carefully introduced pairs only" },
        ["Eclectus Parrot"]          = new() { SocialNeeds = SocialStructure.PairsOk,   IsTerritorial = false, IsPredatory = false, CohabNote = "Bonded pairs; same-sex pairs can be aggressive" },
        ["Senegal Parrot"]           = new() { SocialNeeds = SocialStructure.PairsOk,   IsTerritorial = true,  IsPredatory = false, CohabNote = "Territorial with strangers" },
        ["Indian Ringneck Parakeet"] = new() { SocialNeeds = SocialStructure.PairsOk,   IsTerritorial = true,  IsPredatory = false, GenderRule = GenderPairingRule.OneMalePerEnclosure, CohabNote = "Males can be aggressive with each other — one male per enclosure" },
        ["Quaker Parrot"]            = new() { SocialNeeds = SocialStructure.GroupsOk,  IsTerritorial = false, IsPredatory = false, CohabNote = "Colonial nester; does well in groups" },
        ["Canary"]                   = new() { SocialNeeds = SocialStructure.PairsOk,   IsTerritorial = true,  IsPredatory = false, GenderRule = GenderPairingRule.OneMalePerEnclosure, MaleMaleIsLethal = true, CohabNote = "Male canaries fight and inhibit each other's singing — one male only" },
        ["Zebra Finch"]              = new() { SocialNeeds = SocialStructure.Community, IsTerritorial = false, IsPredatory = false, CohabNote = "Highly social; keep in groups of 4+" },
        ["Ringneck Dove"]            = new() { SocialNeeds = SocialStructure.GroupsOk,  IsTerritorial = false, IsPredatory = false, CohabNote = "Social; pairs or small groups" },

        // ── Fish ──────────────────────────────────────────────────────────────
        ["Betta Fish"]               = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = true,  IsPredatory = false, CohabNote = "Males fight to death; never two males" },
        ["Goldfish"]                 = new() { SocialNeeds = SocialStructure.Community, IsTerritorial = false, IsPredatory = false, CohabNote = "Keep with other goldfish or similarly-sized coldwater fish" },
        ["Koi"]                      = new() { SocialNeeds = SocialStructure.Community, IsTerritorial = false, IsPredatory = false, CohabNote = "Peaceful pond fish" },
        ["Guppy"]                    = new() { SocialNeeds = SocialStructure.Community, IsTerritorial = false, IsPredatory = false, CohabNote = "Peaceful community fish" },
        ["Neon Tetra"]               = new() { SocialNeeds = SocialStructure.Community, IsTerritorial = false, IsPredatory = false, CohabNote = "Schooling; keep 6+ minimum" },
        ["Angelfish"]                = new() { SocialNeeds = SocialStructure.Community, IsTerritorial = true,  IsPredatory = true,  CohabNote = "Will eat neon tetras and other small fish" },
        ["Oscar"]                    = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = true,  IsPredatory = true,  CohabNote = "Highly aggressive; will eat most fish that fit in its mouth" },
        ["Discus"]                   = new() { SocialNeeds = SocialStructure.GroupsOk,  IsTerritorial = false, IsPredatory = false, GenderRule = GenderPairingRule.OneMalePerEnclosure, CohabNote = "One dominant male per group; additional males are excluded aggressively" },
        ["Clownfish"]                = new() { SocialNeeds = SocialStructure.PairsOk,   IsTerritorial = true,  IsPredatory = false, GenderRule = GenderPairingRule.OneMalePerEnclosure, CohabNote = "Only one mated pair per enclosure — additional males will be attacked" },
        ["Blue Tang"]                = new() { SocialNeeds = SocialStructure.Community, IsTerritorial = false, IsPredatory = false, CohabNote = "Reef-safe; one per tank unless very large" },
        ["Common Pleco"]             = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = true,  IsPredatory = false, CohabNote = "Territorial with own species; fine with other fish" },
        ["Corydoras Catfish"]        = new() { SocialNeeds = SocialStructure.Community, IsTerritorial = false, IsPredatory = false, CohabNote = "Schooling bottom-dweller; keep 6+ of same species" },
        ["Arowana"]                  = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = true,  IsPredatory = true,  CohabNote = "Will eat smaller fish and anything near the surface" },
        ["African Cichlid"]          = new() { SocialNeeds = SocialStructure.GroupsOk,  IsTerritorial = true,  IsPredatory = false, GenderRule = GenderPairingRule.OneMalePerEnclosure, MaleMaleIsLethal = true, CohabNote = "Dominant males will kill subordinate males — only one male per tank" },

        // ── Mammals ───────────────────────────────────────────────────────────
        ["African Pygmy Hedgehog"]   = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = true,  IsPredatory = false },
        ["Sugar Glider"]             = new() { SocialNeeds = SocialStructure.GroupsOk,  IsTerritorial = false, IsPredatory = false, CohabNote = "Highly social; should never be kept alone" },
        ["Chinchilla"]               = new() { SocialNeeds = SocialStructure.PairsOk,   IsTerritorial = false, IsPredatory = false, GenderRule = GenderPairingRule.OneMalePerEnclosure, CohabNote = "Male pairs can coexist but may fight when mature — same-sex female pairs are more stable" },
        ["Ferret"]                   = new() { SocialNeeds = SocialStructure.GroupsOk,  IsTerritorial = false, IsPredatory = true,  CohabNote = "Social; groups preferred; predatory instinct with small animals" },
        ["Guinea Pig"]               = new() { SocialNeeds = SocialStructure.GroupsOk,  IsTerritorial = false, IsPredatory = false, CohabNote = "Highly social; should be kept in pairs or groups" },
        ["Fancy Rat"]                = new() { SocialNeeds = SocialStructure.Community, IsTerritorial = false, IsPredatory = false, CohabNote = "Highly social; must be kept in groups of 2+" },
        ["Hamster"]                  = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = true,  IsPredatory = false, CohabNote = "Syrian hamsters fight to the death if housed together" },
        ["Gerbil"]                   = new() { SocialNeeds = SocialStructure.PairsOk,   IsTerritorial = false, IsPredatory = false, GenderRule = GenderPairingRule.OneMalePerEnclosure, CohabNote = "Adult male pairs can fight when mature — same-litter pairs are more stable" },
        ["Degu"]                     = new() { SocialNeeds = SocialStructure.GroupsOk,  IsTerritorial = false, IsPredatory = false, CohabNote = "Highly social; keep in groups of 3+" },
        ["Rabbit"]                   = new() { SocialNeeds = SocialStructure.PairsOk,   IsTerritorial = false, IsPredatory = false, GenderRule = GenderPairingRule.OneMalePerEnclosure, CohabNote = "Unneutered males fight and will breed constantly with females — spay/neuter required" },
        ["Short-tailed Opossum"]     = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = true,  IsPredatory = false },

        // ── Invertebrates ─────────────────────────────────────────────────────
        ["Mexican Red Knee Tarantula"]    = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = true,  IsPredatory = true },
        ["Chilean Rose Tarantula"]        = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = true,  IsPredatory = true },
        ["Chaco Golden Knee Tarantula"]   = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = true,  IsPredatory = true },
        ["Emperor Scorpion"]              = new() { SocialNeeds = SocialStructure.GroupsOk,  IsTerritorial = false, IsPredatory = true,  CohabNote = "One of the few communal scorpion species; groups of same species" },
        ["Praying Mantis"]                = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = false, IsPredatory = true,  CohabNote = "Cannibalistic" },
        ["Giant African Millipede"]       = new() { SocialNeeds = SocialStructure.Community, IsTerritorial = false, IsPredatory = false, CohabNote = "Can be kept in groups; non-aggressive" },
        ["Madagascar Hissing Cockroach"]  = new() { SocialNeeds = SocialStructure.Community, IsTerritorial = false, IsPredatory = false, CohabNote = "Colony species; thrive in groups" },
        ["Indian Stick Insect"]           = new() { SocialNeeds = SocialStructure.Community, IsTerritorial = false, IsPredatory = false, CohabNote = "Can be kept in groups; non-aggressive" },
        ["Land Hermit Crab"]              = new() { SocialNeeds = SocialStructure.Community, IsTerritorial = false, IsPredatory = false, CohabNote = "Social; keep in groups of 3+" },
        ["Vinegaroon"]                    = new() { SocialNeeds = SocialStructure.SoloOnly,  IsTerritorial = true,  IsPredatory = true },
    };

    public static CohabitationProfile? Get(string? commonName)
        => !string.IsNullOrEmpty(commonName) &&
           _profiles.TryGetValue(commonName, out var p) ? p : null;

    /// <summary>
    /// Runs a cohabitation check entirely client-side using the catalog.
    /// No server calls needed — instant results.
    /// </summary>
    public static CohabitationCheckResultDto CheckLocal(CritterDto incoming, IEnumerable<CritterDto> residents)
    {
        var inProfile = Get(incoming.Species);
        var inInfo    = new SpeciesInfo { CommonName = incoming.Species, Type = incoming.SpeciesType, CohabProfile = inProfile };
        var pairs     = residents
            .Select(r =>
            {
                var rp = Get(r.Species);
                var ri = new SpeciesInfo { CommonName = r.Species, Type = r.SpeciesType, CohabProfile = rp };
                return (r.Name, ri, r.Sex);
            })
            .ToList();
        if (pairs.Count == 0)
            return new CohabitationCheckResultDto { CanCohabit = true, HasWarnings = false, Conflicts = [] };
        return CohabitationChecker.Check(inInfo, incoming.Sex, pairs);
    }
}
