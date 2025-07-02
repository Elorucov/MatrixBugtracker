using MatrixBugtracker.BL.Services.Abstractions;

namespace MatrixBugtracker.BL.Services.Implementations
{
    public class Generator : IGenerator
    {
        public string GenerateDigitsCode()
        {
            Random random = new Random();
            int number = random.Next(100000, 999999);
            return number.ToString();
        }
    }
}
