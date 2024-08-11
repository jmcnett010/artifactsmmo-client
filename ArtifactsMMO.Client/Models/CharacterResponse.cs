using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client.Models;

public class CharacterResponse 
{
    [JsonPropertyName("data")]
    public Character Character { get; set; } = new Character();

    public static CharacterResponse Parse(string responseJson)
    {
        return JsonSerializer.Deserialize<CharacterResponse>(responseJson) ?? new CharacterResponse();
    }

    public string ToJson() 
    {
        return JsonSerializer.Serialize(this);
    }
}