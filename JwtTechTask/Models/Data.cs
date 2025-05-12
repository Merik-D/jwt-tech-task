using System.Text.Json.Serialization;

namespace JwtTechTask;

public class Data
{
    
    [JsonPropertyName("data")]
    public List<User> Users { get; set; }
}