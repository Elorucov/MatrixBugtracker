using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.Extensions
{
    public static class ArrayExtensions
    {
        public static void ToLower(this string[] strings)
        {
            for (int i = 0; i < strings.Length; i++)
            {
                strings[i] = strings[i].ToLower();
            }
        }
    }
}
