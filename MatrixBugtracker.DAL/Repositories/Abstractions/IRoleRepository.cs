using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Enums;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.DAL.Repositories.Abstractions
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role> GetByEnumAsync(RoleEnum value);
    }
}
