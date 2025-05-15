using MatrixBugtracker.BL.DTOs.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.DTOs.Products
{
    public class ProductEnumsDTO
    {
        public List<EnumValueDTO> Types { get; init; }
        public List<EnumValueDTO> AccessLevels { get; init; }
    }
}
