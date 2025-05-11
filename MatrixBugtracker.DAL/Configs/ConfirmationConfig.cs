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

            builder.HasIndex(e => e.Email, "UQ_EmailConfirm").IsUnique();

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("email");

            builder.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(6)
                .HasColumnName("code");

            builder.Property(e => e.Kind)
                .IsRequired()
                .HasConversion<byte>()
                .HasColumnName("kind");

            base.Configure(builder);
        }
    }
}
