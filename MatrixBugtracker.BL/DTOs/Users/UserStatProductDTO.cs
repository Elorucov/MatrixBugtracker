using MatrixBugtracker.BL.DTOs.Products;
using MatrixBugtracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.DTOs.Users
{
    public class UserStatProductDTO
    {
        public int ProductId { get; init; }
        public int ReportsCount { get; init; }
    }
}
