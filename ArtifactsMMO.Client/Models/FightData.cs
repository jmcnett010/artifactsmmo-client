using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client.Models;

public class FightData
{
    [JsonPropertyName("xp")]
    public int Xp { get; set; } = 0;
    [JsonPropertyName("gold")]
    public int Gold { get; set; } = 0;

    [JsonPropertyName("logs")]
    public List<string> Character { get; set; } = [""];

    [JsonPropertyName("result")]
    public string Result { get; set; } = "";
}
