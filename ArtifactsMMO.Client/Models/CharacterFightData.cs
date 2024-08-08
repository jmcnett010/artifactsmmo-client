using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client.Models;

public class CharacterFightData
{
    [JsonPropertyName("cooldown")]
    public Cooldown Cooldown { get; set; } = new Cooldown();
    [JsonPropertyName("fight")]
    public FightData Fight { get; set; } = new FightData();

    [JsonPropertyName("character")]
    public object Character { get; set; } = "";
}
