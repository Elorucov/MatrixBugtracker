using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.Domain.Entities.Base
{
    public interface IUpdateEntity : ICreateEntity
    {
        int? UpdateUserId { get; set; }
        DateTime? UpdateTime { get; set; }
    }
}
