using System.Text.Json.Serialization;

namespace JwtTechTask;

public class Meta
{
    [JsonPropertyName("confirmed")]
    public bool Confirmed { get; set; }

    [JsonPropertyName("source")]
    public string Source { get; set; }
    
}