using MatrixBugtracker.DAL.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.DAL.Configs.Base
{
    public class DeleteConfig<T> : CreateConfig<T> where T : class, IDeleteEntity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.IsDeleted).HasColumnName("is_deleted");
            builder.HasQueryFilter(x => x.IsDeleted == false);

            base.Configure(builder);
        }
    }
}
