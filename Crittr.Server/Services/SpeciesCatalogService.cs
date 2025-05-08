using Crittr.Shared.DTOs;
using Crittr.Shared.Models.Enums;

namespace Crittr.Server.Services;

public class SpeciesCatalogService
{
    private readonly List<SpeciesInfo> _species = new()
    {
        new SpeciesInfo
        {
            CommonName = "Leopard Gecko",
            ScientificName = "Eublepharis macularius",
            Type = SpeciesType.Reptile,
            IconUrl = "img/critters/eublepharis-macularius.svg",
            CompatibleEnclosureTypes = new() { EnclosureType.Terrarium }
        },
        new SpeciesInfo
        {
            CommonName = "Ball Python",
            ScientificName = "Python regius",
            Type = SpeciesType.Reptile,
            IconUrl = "img/critters/python-regius.svg",
            CompatibleEnclosureTypes = new() { EnclosureType.Terrarium }
        },
        new SpeciesInfo
        {
            CommonName = "Cockatiel",
            ScientificName = "Nymphicus hollandicus",
            Type = SpeciesType.Bird,
            IconUrl = "img/critters/nymphicus-hollandicus.svg",
            CompatibleEnclosureTypes = new() { EnclosureType.Aviary }
        },
        new SpeciesInfo
        {
            CommonName = "Betta Fish",
            ScientificName = "Betta splendens",
            Type = SpeciesType.Fish,
            IconUrl = "img/critters/betta-splendens.svg",
            CompatibleEnclosureTypes = new() { EnclosureType.Aquarium }
        }
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