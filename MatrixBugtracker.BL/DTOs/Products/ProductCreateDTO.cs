using MatrixBugtracker.BL.Converters;
using MatrixBugtracker.Domain.Enums;
using System.Text.Json.Serialization;

namespace MatrixBugtracker.BL.DTOs.Products
{
    public class ProductCreateDTO
    {
        public string Name { get; init; }
        public string Description { get; init; }

        [JsonConverter(typeof(CustomEnumConverter))]
        public ProductType Type { get; init; }

        [JsonConverter(typeof(CustomEnumConverter))]
        public ProductAccessLevel AccessLevel { get; init; }

        public int? PhotoFileId { get; init; }
    }
}
