using MatrixBugtracker.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatrixBugtracker.DAL.Configs.Base
{
    internal class CreateConfig<T> : EntityConfig<T> where T : class, ICreateEntity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.CreatorId).IsRequired().HasColumnName("creator_id");
            builder.Property(x => x.CreationTime).IsRequired().HasColumnName("creation_time");

            base.Configure(builder);
        }
    }
}