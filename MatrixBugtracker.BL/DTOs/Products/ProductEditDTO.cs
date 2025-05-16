using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.DTOs.Products
{
    public class ProductEditDTO : ProductCreateDTO
    {
        public int Id { get; set; }
    }
}
