using System.Text.Json;
using System.Text.Json.Serialization;

namespace JwtTechTask;

public class StatusEnumConverter : JsonConverter<Status>
{
    public override Status Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var enumText = reader.GetString();
            if (Enum.TryParse(typeof(Status), enumText, ignoreCase: true, out var value))
            {
                return (Status)value;
            }
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            if (reader.TryGetInt32(out int intValue) && Enum.IsDefined(typeof(Status), intValue))
            {
                return (Status)intValue;
            }
        }

        return Status.Unknown;
    }

    public override void Write(Utf8JsonWriter writer, Status value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
