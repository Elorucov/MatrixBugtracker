using MatrixBugtracker.DAL.Configs.Base;
using MatrixBugtracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatrixBugtracker.DAL.Configs
{
    internal class ConfirmationConfig : BaseEntityConfig<Confirmation>
    {
        public override void Configure(EntityTypeBuilder<Confirmation> builder)
        {
            builder.ToTable("confirmations");

            builder.Property(e => e.UserId)
                .IsRequired()
                .HasColumnName("user_id");

            builder.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(6)
                .HasColumnName("code");

            builder.Property(e => e.Kind)
                .IsRequired()
                .HasConversion<byte>()
                .HasColumnName("kind");

            builder.HasOne(d => d.User).WithMany(p => p.Confirmations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Confirmation_User");

            base.Configure(builder);
        }
    }
}
