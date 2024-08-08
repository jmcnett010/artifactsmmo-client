using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client.Models;

public class Cooldown
{
    [JsonPropertyName("total_seconds")]
    public int TotalSeconds { get; set; }
    [JsonPropertyName("remaining_seconds")]
    public int RemainingSeconds { get; set; }

    public static Cooldown ParseResponse(string responseJson)
    {
        return JsonSerializer.Deserialize<Cooldown>(responseJson) ?? new Cooldown();
    }
}
