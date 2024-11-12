using System.Text.Json;
using System.Text.Json.Serialization;

namespace PetGrooming_Management_System.Utils
{
    public class TimeOnlyJsonConverter : JsonConverter<TimeOnly?>
    {
        public override TimeOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String &&
                TimeOnly.TryParse(reader.GetString(), out var time))
            {
                return time;
            }
            return null;
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteStringValue(value.Value.ToString("HH:mm:ss"));
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
