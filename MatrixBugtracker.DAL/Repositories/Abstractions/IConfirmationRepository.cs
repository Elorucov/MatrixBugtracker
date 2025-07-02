using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using MatrixBugtracker.Domain.Entities;
using MatrixBugtracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.DAL.Repositories.Abstractions
{
    public interface IConfirmationRepository : IRepository<Confirmation>
    {
        Task<Confirmation> GetByUserIdAsync(int userId, EmailConfirmationKind kind);
    }
}
