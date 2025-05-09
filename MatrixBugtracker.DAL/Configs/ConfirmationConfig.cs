using MatrixBugtracker.DAL.Configs.Base;
using MatrixBugtracker.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            builder.Property(e => e.Kind)
                .IsRequired()
                .HasConversion<byte>()
                .HasColumnName("kind");

            base.Configure(builder);
        }
    }
}
