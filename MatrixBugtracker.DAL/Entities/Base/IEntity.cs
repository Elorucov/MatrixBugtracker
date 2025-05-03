using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.DAL.Entities.Base
{
    public interface IEntity
    {
        int Id { get; init; }
    }
}
