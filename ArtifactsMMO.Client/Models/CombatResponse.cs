using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client.Models;

public class CombatResults
{
    [JsonPropertyName("data")]
    public CharacterFightData Data { get; set; } = new CharacterFightData();

    public static CombatResults Parse(string responseJson)
    {
        return JsonSerializer.Deserialize<CombatResults>(responseJson) ?? new CombatResults();
    }
}
