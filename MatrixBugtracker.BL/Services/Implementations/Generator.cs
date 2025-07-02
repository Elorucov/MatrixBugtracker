using MatrixBugtracker.BL.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
