using Crittr.Shared.DTOs;
using Crittr.Shared.Models.Enums;

namespace Crittr.Server.Services;

public class SpeciesCatalogService
{
    private readonly List<SpeciesInfo> _species = new()
    {
        // ── Reptiles ────────────────────────────────────────────────────────
        new SpeciesInfo { CommonName = "Leopard Gecko",           ScientificName = "Eublepharis macularius",       Type = SpeciesType.Reptile, IconUrl = "img/critters/eublepharis-macularius.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium } },
        new SpeciesInfo { CommonName = "Ball Python",             ScientificName = "Python regius",                Type = SpeciesType.Reptile, IconUrl = "img/critters/python-regius.svg",           CompatibleEnclosureTypes = new() { EnclosureType.Terrarium } },
        new SpeciesInfo { CommonName = "Bearded Dragon",          ScientificName = "Pogona vitticeps",             Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium } },
        new SpeciesInfo { CommonName = "Crested Gecko",           ScientificName = "Correlophus ciliatus",         Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium, EnclosureType.Vivarium } },
        new SpeciesInfo { CommonName = "Corn Snake",              ScientificName = "Pantherophis guttatus",        Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium } },
        new SpeciesInfo { CommonName = "Blue-tongued Skink",      ScientificName = "Tiliqua scincoides",           Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium } },
        new SpeciesInfo { CommonName = "Veiled Chameleon",        ScientificName = "Chamaeleo calyptratus",        Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium, EnclosureType.Vivarium } },
        new SpeciesInfo { CommonName = "Panther Chameleon",       ScientificName = "Furcifer pardalis",            Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Vivarium } },
        new SpeciesInfo { CommonName = "Jackson's Chameleon",     ScientificName = "Trioceros jacksonii",          Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Vivarium } },
        new SpeciesInfo { CommonName = "Green Iguana",            ScientificName = "Iguana iguana",                Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium, EnclosureType.Vivarium } },
        new SpeciesInfo { CommonName = "Chinese Water Dragon",    ScientificName = "Physignathus cocincinus",      Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Vivarium } },
        new SpeciesInfo { CommonName = "Uromastyx",               ScientificName = "Uromastyx ornata",             Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium } },
        new SpeciesInfo { CommonName = "Argentine Black and White Tegu", ScientificName = "Salvator merianae",    Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium } },
        new SpeciesInfo { CommonName = "Ackie Monitor",           ScientificName = "Varanus acanthurus",           Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium } },
        new SpeciesInfo { CommonName = "Savannah Monitor",        ScientificName = "Varanus exanthematicus",       Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium } },
        new SpeciesInfo { CommonName = "Gargoyle Gecko",          ScientificName = "Rhacodactylus auriculatus",    Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium, EnclosureType.Vivarium } },
        new SpeciesInfo { CommonName = "African Fat-tailed Gecko", ScientificName = "Hemitheconyx caudicinctus",  Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium } },
        new SpeciesInfo { CommonName = "Giant Day Gecko",         ScientificName = "Phelsuma grandis",             Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Vivarium } },
        new SpeciesInfo { CommonName = "Tokay Gecko",             ScientificName = "Gekko gecko",                  Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium, EnclosureType.Vivarium } },
        new SpeciesInfo { CommonName = "Kenyan Sand Boa",         ScientificName = "Eryx colubrinus loveridgei",  Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium, EnclosureType.Bin } },
        new SpeciesInfo { CommonName = "Hognose Snake",           ScientificName = "Heterodon nasicus",            Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium } },
        new SpeciesInfo { CommonName = "Boa Constrictor",         ScientificName = "Boa constrictor",              Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium } },
        new SpeciesInfo { CommonName = "Carpet Python",           ScientificName = "Morelia spilota",              Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium } },
        new SpeciesInfo { CommonName = "Green Tree Python",       ScientificName = "Morelia viridis",              Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium, EnclosureType.Vivarium } },
        new SpeciesInfo { CommonName = "Blood Python",            ScientificName = "Python brongersmai",           Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium } },
        new SpeciesInfo { CommonName = "King Snake",              ScientificName = "Lampropeltis getula",          Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium } },
        new SpeciesInfo { CommonName = "Milk Snake",              ScientificName = "Lampropeltis triangulum",      Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium } },
        new SpeciesInfo { CommonName = "Russian Tortoise",        ScientificName = "Testudo horsfieldii",          Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium } },
        new SpeciesInfo { CommonName = "Hermann's Tortoise",      ScientificName = "Testudo hermanni",             Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium } },
        new SpeciesInfo { CommonName = "Sulcata Tortoise",        ScientificName = "Centrochelys sulcata",         Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium, EnclosureType.FreeRoamRoom } },
        new SpeciesInfo { CommonName = "Red-eared Slider",        ScientificName = "Trachemys scripta elegans",    Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Tank, EnclosureType.Aquarium } },
        new SpeciesInfo { CommonName = "Box Turtle",              ScientificName = "Terrapene carolina",           Type = SpeciesType.Reptile, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium } },

        // ── Amphibians ──────────────────────────────────────────────────────
        new SpeciesInfo { CommonName = "Axolotl",                 ScientificName = "Ambystoma mexicanum",          Type = SpeciesType.Amphibian, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Aquarium } },
        new SpeciesInfo { CommonName = "Pacman Frog",             ScientificName = "Ceratophrys ornata",           Type = SpeciesType.Amphibian, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium } },
        new SpeciesInfo { CommonName = "White's Tree Frog",       ScientificName = "Litoria caerulea",             Type = SpeciesType.Amphibian, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Vivarium, EnclosureType.Terrarium } },
        new SpeciesInfo { CommonName = "Red-eyed Tree Frog",      ScientificName = "Agalychnis callidryas",        Type = SpeciesType.Amphibian, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Vivarium } },
        new SpeciesInfo { CommonName = "Poison Dart Frog",        ScientificName = "Dendrobates tinctorius",       Type = SpeciesType.Amphibian, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Vivarium } },
        new SpeciesInfo { CommonName = "African Bullfrog",        ScientificName = "Pyxicephalus adspersus",       Type = SpeciesType.Amphibian, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium } },
        new SpeciesInfo { CommonName = "Fire-bellied Toad",       ScientificName = "Bombina orientalis",           Type = SpeciesType.Amphibian, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Paludarium, EnclosureType.Terrarium } },
        new SpeciesInfo { CommonName = "Tomato Frog",             ScientificName = "Dyscophus antongilii",         Type = SpeciesType.Amphibian, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium } },
        new SpeciesInfo { CommonName = "Tiger Salamander",        ScientificName = "Ambystoma tigrinum",           Type = SpeciesType.Amphibian, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium } },
        new SpeciesInfo { CommonName = "Fire Salamander",         ScientificName = "Salamandra salamandra",        Type = SpeciesType.Amphibian, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium, EnclosureType.Vivarium } },

        // ── Birds ────────────────────────────────────────────────────────────
        new SpeciesInfo { CommonName = "Cockatiel",               ScientificName = "Nymphicus hollandicus",        Type = SpeciesType.Bird, IconUrl = "img/critters/nymphicus-hollandicus.svg", CompatibleEnclosureTypes = new() { EnclosureType.Cage, EnclosureType.Aviary } },
        new SpeciesInfo { CommonName = "Budgerigar",              ScientificName = "Melopsittacus undulatus",      Type = SpeciesType.Bird, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Cage, EnclosureType.Aviary } },
        new SpeciesInfo { CommonName = "African Grey Parrot",     ScientificName = "Psittacus erithacus",          Type = SpeciesType.Bird, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Cage } },
        new SpeciesInfo { CommonName = "Blue and Gold Macaw",     ScientificName = "Ara ararauna",                 Type = SpeciesType.Bird, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Cage, EnclosureType.Aviary } },
        new SpeciesInfo { CommonName = "Green-cheeked Conure",    ScientificName = "Pyrrhura molinae",             Type = SpeciesType.Bird, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Cage } },
        new SpeciesInfo { CommonName = "Sun Conure",              ScientificName = "Aratinga solstitialis",        Type = SpeciesType.Bird, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Cage } },
        new SpeciesInfo { CommonName = "Lovebird",                ScientificName = "Agapornis roseicollis",        Type = SpeciesType.Bird, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Cage, EnclosureType.Aviary } },
        new SpeciesInfo { CommonName = "Caique",                  ScientificName = "Pionites melanocephalus",      Type = SpeciesType.Bird, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Cage } },
        new SpeciesInfo { CommonName = "Eclectus Parrot",         ScientificName = "Eclectus roratus",             Type = SpeciesType.Bird, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Cage, EnclosureType.Aviary } },
        new SpeciesInfo { CommonName = "Senegal Parrot",          ScientificName = "Poicephalus senegalus",        Type = SpeciesType.Bird, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Cage } },
        new SpeciesInfo { CommonName = "Indian Ringneck Parakeet", ScientificName = "Psittacula krameri",          Type = SpeciesType.Bird, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Cage, EnclosureType.Aviary } },
        new SpeciesInfo { CommonName = "Quaker Parrot",           ScientificName = "Myiopsitta monachus",          Type = SpeciesType.Bird, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Cage } },
        new SpeciesInfo { CommonName = "Canary",                  ScientificName = "Serinus canaria",              Type = SpeciesType.Bird, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Cage, EnclosureType.Aviary } },
        new SpeciesInfo { CommonName = "Zebra Finch",             ScientificName = "Taeniopygia guttata",          Type = SpeciesType.Bird, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Cage, EnclosureType.Aviary } },
        new SpeciesInfo { CommonName = "Ringneck Dove",           ScientificName = "Streptopelia risoria",         Type = SpeciesType.Bird, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Cage, EnclosureType.Aviary } },

        // ── Fish ─────────────────────────────────────────────────────────────
        new SpeciesInfo { CommonName = "Betta Fish",              ScientificName = "Betta splendens",              Type = SpeciesType.Fish, IconUrl = "img/critters/betta-splendens.svg", CompatibleEnclosureTypes = new() { EnclosureType.Aquarium } },
        new SpeciesInfo { CommonName = "Goldfish",                ScientificName = "Carassius auratus",            Type = SpeciesType.Fish, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Aquarium, EnclosureType.Tank } },
        new SpeciesInfo { CommonName = "Koi",                     ScientificName = "Cyprinus rubrofuscus",         Type = SpeciesType.Fish, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Tank, EnclosureType.Aquarium } },
        new SpeciesInfo { CommonName = "Guppy",                   ScientificName = "Poecilia reticulata",          Type = SpeciesType.Fish, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Aquarium } },
        new SpeciesInfo { CommonName = "Neon Tetra",              ScientificName = "Paracheirodon innesi",         Type = SpeciesType.Fish, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Aquarium } },
        new SpeciesInfo { CommonName = "Angelfish",               ScientificName = "Pterophyllum scalare",         Type = SpeciesType.Fish, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Aquarium } },
        new SpeciesInfo { CommonName = "Oscar",                   ScientificName = "Astronotus ocellatus",         Type = SpeciesType.Fish, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Aquarium } },
        new SpeciesInfo { CommonName = "Discus",                  ScientificName = "Symphysodon aequifasciatus",   Type = SpeciesType.Fish, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Aquarium } },
        new SpeciesInfo { CommonName = "Clownfish",               ScientificName = "Amphiprion ocellaris",         Type = SpeciesType.Fish, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Aquarium } },
        new SpeciesInfo { CommonName = "Blue Tang",               ScientificName = "Paracanthurus hepatus",        Type = SpeciesType.Fish, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Aquarium } },
        new SpeciesInfo { CommonName = "Common Pleco",            ScientificName = "Hypostomus plecostomus",       Type = SpeciesType.Fish, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Aquarium } },
        new SpeciesInfo { CommonName = "Corydoras Catfish",       ScientificName = "Corydoras paleatus",           Type = SpeciesType.Fish, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Aquarium } },
        new SpeciesInfo { CommonName = "Arowana",                 ScientificName = "Osteoglossum bicirrhosum",     Type = SpeciesType.Fish, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Aquarium } },
        new SpeciesInfo { CommonName = "African Cichlid",         ScientificName = "Metriaclima estherae",         Type = SpeciesType.Fish, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Aquarium } },

        // ── Mammals ──────────────────────────────────────────────────────────
        new SpeciesInfo { CommonName = "African Pygmy Hedgehog",  ScientificName = "Atelerix albiventris",         Type = SpeciesType.Mammal, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Cage } },
        new SpeciesInfo { CommonName = "Sugar Glider",            ScientificName = "Petaurus breviceps",           Type = SpeciesType.Mammal, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Cage } },
        new SpeciesInfo { CommonName = "Chinchilla",              ScientificName = "Chinchilla lanigera",          Type = SpeciesType.Mammal, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Cage } },
        new SpeciesInfo { CommonName = "Ferret",                  ScientificName = "Mustela putorius furo",        Type = SpeciesType.Mammal, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Cage, EnclosureType.FreeRoamRoom } },
        new SpeciesInfo { CommonName = "Guinea Pig",              ScientificName = "Cavia porcellus",              Type = SpeciesType.Mammal, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Cage } },
        new SpeciesInfo { CommonName = "Fancy Rat",               ScientificName = "Rattus norvegicus domestica",  Type = SpeciesType.Mammal, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Cage } },
        new SpeciesInfo { CommonName = "Hamster",                 ScientificName = "Mesocricetus auratus",         Type = SpeciesType.Mammal, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Cage, EnclosureType.Bin } },
        new SpeciesInfo { CommonName = "Gerbil",                  ScientificName = "Meriones unguiculatus",        Type = SpeciesType.Mammal, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Cage, EnclosureType.Bin } },
        new SpeciesInfo { CommonName = "Degu",                    ScientificName = "Octodon degus",                Type = SpeciesType.Mammal, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Cage } },
        new SpeciesInfo { CommonName = "Rabbit",                  ScientificName = "Oryctolagus cuniculus",        Type = SpeciesType.Mammal, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Cage, EnclosureType.FreeRoamRoom } },
        new SpeciesInfo { CommonName = "Short-tailed Opossum",    ScientificName = "Monodelphis domestica",        Type = SpeciesType.Mammal, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Cage } },

        // ── Invertebrates ────────────────────────────────────────────────────
        new SpeciesInfo { CommonName = "Mexican Red Knee Tarantula", ScientificName = "Brachypelma hamorii",       Type = SpeciesType.Invertebrate, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium, EnclosureType.Bin } },
        new SpeciesInfo { CommonName = "Chilean Rose Tarantula",   ScientificName = "Grammostola rosea",           Type = SpeciesType.Invertebrate, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium, EnclosureType.Bin } },
        new SpeciesInfo { CommonName = "Chaco Golden Knee Tarantula", ScientificName = "Grammostola pulchripes",   Type = SpeciesType.Invertebrate, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium, EnclosureType.Bin } },
        new SpeciesInfo { CommonName = "Emperor Scorpion",         ScientificName = "Pandinus imperator",          Type = SpeciesType.Invertebrate, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium, EnclosureType.Insectarium } },
        new SpeciesInfo { CommonName = "Praying Mantis",           ScientificName = "Tenodera sinensis",           Type = SpeciesType.Invertebrate, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Insectarium, EnclosureType.Terrarium } },
        new SpeciesInfo { CommonName = "Giant African Millipede",  ScientificName = "Archispirostreptus gigas",    Type = SpeciesType.Invertebrate, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium, EnclosureType.Insectarium } },
        new SpeciesInfo { CommonName = "Madagascar Hissing Cockroach", ScientificName = "Gromphadorhina portentosa", Type = SpeciesType.Invertebrate, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Insectarium, EnclosureType.Bin } },
        new SpeciesInfo { CommonName = "Indian Stick Insect",      ScientificName = "Carausius morosus",           Type = SpeciesType.Invertebrate, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Insectarium, EnclosureType.Terrarium } },
        new SpeciesInfo { CommonName = "Land Hermit Crab",         ScientificName = "Coenobita clypeatus",         Type = SpeciesType.Invertebrate, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium } },
        new SpeciesInfo { CommonName = "Vinegaroon",               ScientificName = "Mastigoproctus giganteus",    Type = SpeciesType.Invertebrate, IconUrl = "img/critters/default.svg", CompatibleEnclosureTypes = new() { EnclosureType.Terrarium, EnclosureType.Insectarium } },
    };

    public Task<List<SpeciesInfo>> GetAllAsync() => Task.FromResult(_species);

    public Task<List<SpeciesInfo>> SearchAsync(string query, SpeciesType? type = null)
    {
        var result = _species
            .Where(s =>
                s.CommonName.Contains(query, StringComparison.OrdinalIgnoreCase) &&
                (type == null || s.Type == type))
            .ToList();

        return Task.FromResult(result);
    }
}