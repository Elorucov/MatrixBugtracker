using MatrixBugtracker.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatrixBugtracker.DAL.Configs.Base
{
    internal class DeleteConfig<T> : UpdateConfig<T> where T : class, IDeleteEntity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.IsDeleted).IsRequired().HasColumnName("is_deleted");
            builder.Property(x => x.DeletedByUserId).HasColumnName("deleted_by_user_id");
            builder.Property(x => x.DeletionTime).HasColumnName("deletion_time");
            builder.HasQueryFilter(x => x.IsDeleted == false);

            base.Configure(builder);
        }
    }
}
