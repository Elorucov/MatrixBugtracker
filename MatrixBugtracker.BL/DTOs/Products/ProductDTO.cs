using MatrixBugtracker.BL.Converters;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.Domain.Enums;
using System.Text.Json.Serialization;

namespace MatrixBugtracker.BL.DTOs.Products
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public FileDTO Photo { get; set; }

        [JsonConverter(typeof(CustomEnumConverter))]
        public ProductAccessLevel AccessLevel { get; set; }

        [JsonConverter(typeof(CustomEnumConverter))]
        public ProductType Type { get; set; }

        [JsonConverter(typeof(CustomEnumConverter))]
        public ProductMemberStatus MembershipStatus { get; set; }

        public bool IsOver { get; set; }

        #region Extended (only when getting single product by id)

        public ProductCountersDTO Counters { get; set; }

        #endregion
    }
}
