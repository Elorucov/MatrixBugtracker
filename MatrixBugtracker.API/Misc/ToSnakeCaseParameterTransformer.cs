using System.Text.RegularExpressions;

namespace MatrixBugtracker.API.Misc
{
    public class ToSnakeCaseParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value) => value != null
            ? Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1_$2").ToLower() // to snake case
            : null;
    }
}
