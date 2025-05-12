using System.Text.Json.Serialization;

namespace JwtTechTask;

public class User
{
    [JsonPropertyName("userId")]
    public string Id { get; set; }

    [JsonPropertyName("transactions")]
    public List<Transaction> Transactions { get; set; }
}