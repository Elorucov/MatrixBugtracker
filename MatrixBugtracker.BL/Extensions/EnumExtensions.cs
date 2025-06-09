using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.Resources;
using System.Text;

namespace MatrixBugtracker.BL.Extensions
{
    public static class EnumExtensions
    {
        public static string GetValuesCommaSeparated<T>() where T : struct, Enum
        {
            StringBuilder sb = new StringBuilder();
            var values = Enum.GetValues<T>();
            decimal[] nums = new decimal[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                var value = values[i];
                var num = Convert.ToDecimal(value);
                nums[i] = num;
            }

            return string.Join(", ", nums);
        }

        public static List<EnumValueDTO> GetTranslatedEnums<T>() where T : struct, Enum
        {
            return Enum.GetValues<T>()
                .Select(e => new EnumValueDTO(Convert.ToByte(e), EnumValues.ResourceManager.GetString($"{typeof(T).Name}_{e}"))).ToList();
        }

        public static EnumValueDTO GetTranslatedEnum<T>(this T value) where T : struct, Enum
        {
            return new EnumValueDTO(Convert.ToByte(value), EnumValues.ResourceManager.GetString($"{typeof(T).Name}_{value}"));
        }
    }
}
