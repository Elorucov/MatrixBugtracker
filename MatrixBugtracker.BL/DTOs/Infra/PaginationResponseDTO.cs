using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.DTOs.Infra
{
    public class PaginationResponseDTO<T> : ResponseDTO<List<T>>
    {
        public int? Count { get; private set; }

        public PaginationResponseDTO(List<T> response, int count, int httpStatusCode = 200) : base(response, httpStatusCode)
        {
            Count = count;
        }
    }
}
