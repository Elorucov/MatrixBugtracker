using MatrixBugtracker.DAL.Entities.Base;
using MatrixBugtracker.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.DAL.Entities
{
    public class Confirmation : BaseEntity
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public EmailConfirmationKind Kind { get; set; }
    }
}
