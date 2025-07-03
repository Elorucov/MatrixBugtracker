using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.DTOs.Products
{
    public class GetProductsRequestDTO : PaginatedSearchRequestDTO
    {
        public ProductType? Type { get; init; }
    }
}
