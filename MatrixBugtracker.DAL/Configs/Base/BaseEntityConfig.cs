using MatrixBugtracker.DAL.Entities.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatrixBugtracker.DAL.Configs.Base
{
    internal class BaseEntityConfig<T> : DeleteConfig<T> where T : BaseEntity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
        }
    }

    // required, because ICreateEntity config is ignored
    internal class BaseEntityConfig2<T> : CreateConfig<T> where T : BaseEntity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
        }
    }
}
