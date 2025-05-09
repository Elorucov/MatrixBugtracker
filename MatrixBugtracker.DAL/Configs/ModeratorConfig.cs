using MatrixBugtracker.DAL.Configs.Base;
using MatrixBugtracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatrixBugtracker.DAL.Configs
{
    internal class ModeratorConfig : DeleteConfig<Moderator>
    {
        public override void Configure(EntityTypeBuilder<Moderator> builder)
        {
            builder.ToTable("moderators");

            builder.HasIndex(e => e.UserId, "UQ_Moder").IsUnique();
            builder.Property(e => e.UserId).HasColumnName("user_id");

            builder.HasOne(d => d.User).WithOne(p => p.Moderator)
                .HasForeignKey<Moderator>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Moder_Id");

            base.Configure(builder);
        }
    }
}
