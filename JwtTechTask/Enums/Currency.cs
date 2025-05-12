using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace JwtTechTask;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Currency
{
    [EnumMember(Value = "UAH")]
    UAH,

    [EnumMember(Value = "EUR")]
    EUR,
    
    [EnumMember(Value = "USD")]
    USD
}