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
    public class CreateConfig<T> : EntityConfig<T> where T : class, ICreateEntity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.CreateTime).HasColumnName("create_time");

            base.Configure(builder);
        }
    }
}
