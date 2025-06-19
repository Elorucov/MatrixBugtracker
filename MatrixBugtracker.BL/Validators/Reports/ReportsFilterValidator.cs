using FluentValidation;
using MatrixBugtracker.BL.DTOs.Reports;
using MatrixBugtracker.BL.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.Validators.Reports
{
    public class ReportsFilterValidator : AbstractValidator<GetReportsRequestDTO>
    {
        public ReportsFilterValidator()
        {
            RuleFor(p => p.Number).GreaterThan(0);
            RuleFor(p => p.Size).GreaterThan(0);
            RuleFor(p => p.ProductId).GreaterThanOrEqualTo(0);
            RuleFor(p => p.CreatorId).GreaterThanOrEqualTo(0);
        }
    }
}
