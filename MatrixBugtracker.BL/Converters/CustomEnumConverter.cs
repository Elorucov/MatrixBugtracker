using MatrixBugtracker.BL.Extensions;
using MatrixBugtracker.BL.Resources;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MatrixBugtracker.BL.Converters
{
    public class CustomEnumConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum;

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) =>
            (JsonConverter)Activator.CreateInstance(typeof(CustomConverter<>).MakeGenericType(typeToConvert))!;

        class CustomConverter<T> : JsonConverter<T> where T : struct, Enum
        {
            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string valueStr = reader.GetString();
                if (!Enum.TryParse<T>(valueStr, out var result))
                {
                    var availableValues = EnumExtensions.GetStringValuesCommaSeparated<T>();
                    throw new JsonException(string.Format(Errors.InvalidEnum, availableValues));
                }
                return result;
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(Enum.GetName(value));
            }
        }
    }
}
