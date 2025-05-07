using System.Net.Http.Json;
using System.Text.Json.Serialization;
using ReptileCare.Shared.Models.Enums;

namespace ReptileCare.Client.Services;

public class SpeciesService
{
    private readonly HttpClient _http;

    private static readonly Dictionary<SpeciesType, int> TaxonMap = new()
    {
        { SpeciesType.Reptile, 26036 },      // Reptilia
        { SpeciesType.Amphibian, 20978 },    // Amphibia
        { SpeciesType.Fish, 47178 },         // Actinopterygii
        { SpeciesType.Insect, 47158 },       // Insecta
        { SpeciesType.Arachnid, 47119 },     // Arachnida
        { SpeciesType.Crustacean, 48325 },   // Crustacea
        { SpeciesType.Bird, 3 },             // Aves
        { SpeciesType.Rodent, 43640 },       // Rodentia
        { SpeciesType.SmallMammal, 40151 },  // Approximate fallback
        { SpeciesType.ExoticMammal, 40151 }, // Use Mammalia or relevant taxon
        { SpeciesType.Mollusk, 47115 },      // Mollusca
        { SpeciesType.Myriapod, 372739 }     // Myriapoda
    };

    public SpeciesService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<string>> GetTopSpeciesByTypeAsync(string type)
    {
        var response = await _http.GetFromJsonAsync<INatResponse>(
            $"https://api.inaturalist.org/v1/taxa?q={type}&rank=species&per_page=50");

        return response?.Results?
            .Select(r => r.Name)?
            .Distinct()?
            .ToList() ?? new();
    }
    
    public async Task<List<SpeciesSuggestion>> SearchSpeciesAsync(SpeciesType type, string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return new();

        int taxonId = TaxonMap.TryGetValue(type, out var id) ? id : 0;

        var url = $"https://api.inaturalist.org/v1/taxa?q={Uri.EscapeDataString(query)}&rank=species&per_page=100";
        if (taxonId > 0) url += $"&taxon_id={taxonId}";

        var result = await _http.GetFromJsonAsync<INatResponse>(url);

        return result?.Results?
            .Select(r => new SpeciesSuggestion
            {
                CommonName = string.IsNullOrWhiteSpace(r.PreferredCommonName) ? $"(no common name)" : r.PreferredCommonName,
                ScientificName = r.Name,
                ImageUrl = r.DefaultPhoto?.SquareUrl ?? ""
            })
            .ToList() ?? new();
    }
    
    private class INatResponse
    {
        public List<Taxon> Results { get; set; } = new();

        public class Taxon
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("preferred_common_name")]
            public string PreferredCommonName { get; set; }

            [JsonPropertyName("default_photo")]
            public Photo DefaultPhoto { get; set; }
        }

        public class Photo
        {
            [JsonPropertyName("square_url")]
            public string SquareUrl { get; set; }
        }

    }
    
    public class SpeciesSuggestion
    {
        public string CommonName { get; set; }
        public string ScientificName { get; set; }
        public string ImageUrl { get; set; }
    }
}
