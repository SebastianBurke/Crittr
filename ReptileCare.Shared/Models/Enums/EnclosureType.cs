namespace ReptileCare.Shared.Models.Enums;

public enum EnclosureType
{
    Terrarium,        // Land-based, for reptiles and amphibians
    Aquarium,         // Fully aquatic, for fish, crustaceans, aquatic amphibians
    Paludarium,       // Water + land, for amphibians or semi-aquatic reptiles
    Vivarium,         // Bioactive, living habitat — terrarium variant
    Insectarium,      // For bugs, often vertical or ventilated
    Aviary,           // For birds — standing or hanging cages
    Cage,             // Generic rodent or mammal cage
    Bin,              // Plastic bin setups — common for breeding or budget habitats
    RackSystem,       // Used for breeders — usually reptiles
    FreeRoamRoom,     // For ferrets, rabbits, gliders — supervised open space
    Tank,             // General fallback term — aquarium or terrestrial
    Other             // Catch-all for niche or custom builds
}
