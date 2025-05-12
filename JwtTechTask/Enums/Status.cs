using System.Text.Json.Serialization;

namespace JwtTechTask;


[JsonConverter(typeof(StatusEnumConverter))]
public enum Status
{
    InProgress = 0,
    Completed = 1,
    Unknown = 2
}