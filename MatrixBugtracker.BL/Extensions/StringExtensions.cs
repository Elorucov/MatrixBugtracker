using System.Text;

namespace MatrixBugtracker.BL.Extensions
{
    public static class StringExtensions
    {
        public static string Truncate(this string source, int length)
        {
            if (string.IsNullOrEmpty(source) || source.Length <= length) return source;

            StringBuilder sb = new StringBuilder();
            sb.Append(source.AsSpan().Slice(0, length));
            sb.Append("…");
            return sb.ToString();
        }
    }
}
