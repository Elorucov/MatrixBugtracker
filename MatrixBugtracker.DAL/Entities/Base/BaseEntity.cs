using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.DAL.Entities.Base
{
    public abstract class BaseEntity : IDeleteEntity
    {
        public int Id { get; init; }
        public bool IsDeleted { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
