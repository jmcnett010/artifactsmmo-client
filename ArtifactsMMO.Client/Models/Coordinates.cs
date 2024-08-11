using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client.Models;

public class Coordinates 
{
	[JsonPropertyName("x")]
    public int X { get; set;  }
    
	[JsonPropertyName("y")]
    public int Y { get; set;  }
    
	[JsonPropertyName("name")]
    public string Name { get; set;  } = "";
    
	[JsonPropertyName("skin")]
    public string Skin { get; set;  } = "";
    
	[JsonPropertyName("content")]
    public MapContent? Content { get; set; }

    public override string ToString() 
    {
        return $"({X},{Y})";
    }

    public string ToJson() 
    {
        return JsonSerializer.Serialize(this);
    }
}