using AuthorizationOperation.Domain.Authorization.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorizationOperation.Infrastructure.EF.Config
{
    public class AuthorizationConfiguration : IEntityTypeConfiguration<Authorization>
    {
        public void Configure(EntityTypeBuilder<Authorization> builder)
        {
            builder.ToTable("authorization");

            builder.Property(e => e.Id).HasColumnName("id").IsRequired().HasConversion<int>();
            builder.Property(e => e.StatusId).HasColumnName("status_id").IsRequired().HasConversion<ushort>();
            builder.Property(e => e.Created).HasColumnName("created").IsRequired();
            builder.Property(e => e.Customer).HasColumnName("customer").IsRequired();
            builder.Property(e => e.UUID).HasColumnName("uuid").IsRequired().HasConversion<string>();

            builder.HasKey(e => e.Id).HasName("pk_authorization_id");

            builder.HasIndex(e => e.StatusId).HasDatabaseName("ix_authorization_status_id");

            builder.HasOne(e => e.Status)
                .WithMany()
                .HasForeignKey(s => s.StatusId)
                .HasConstraintName("fk_authorization_authorization_status_status_id");
        }
    }
}
