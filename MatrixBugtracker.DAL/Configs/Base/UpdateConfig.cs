using MatrixBugtracker.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.DAL.Configs.Base
{
    internal class UpdateConfig<T> : CreateConfig<T> where T : class, IUpdateEntity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.UpdateUserId).HasColumnName("update_user_id");
            builder.Property(x => x.UpdateTime).HasColumnName("update_time");

            base.Configure(builder);
        }
    }
}
