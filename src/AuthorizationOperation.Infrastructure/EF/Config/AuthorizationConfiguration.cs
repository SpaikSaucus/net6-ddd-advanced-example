using AuthorizationOperation.Domain.Authorization.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorizationOperation.Infrastructure.EF.Config
{
    public class AuthorizationConfiguration : IEntityTypeConfiguration<Authorization>
    {
        public void Configure(EntityTypeBuilder<Authorization> builder)
        {
            builder.Property(e => e.Id).IsRequired().HasConversion<int>();
            builder.Property(e => e.UUID).IsRequired().HasConversion<string>();
            builder.Property(e => e.StatusId).IsRequired().HasConversion<int>();
            builder.Property(e => e.Created).IsRequired();
            builder.Property(e => e.Customer).IsRequired();

            builder.HasOne(e => e.Status)
                .WithMany()
                .HasForeignKey(s => s.StatusId);
        }
    }
}
