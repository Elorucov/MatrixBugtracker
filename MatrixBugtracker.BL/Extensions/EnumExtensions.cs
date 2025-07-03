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

        public static string GetStringValuesCommaSeparated<T>() where T : struct, Enum
        {
            var names = Enum.GetNames<T>();
            return string.Join(", ", names);
        }
    }
}
