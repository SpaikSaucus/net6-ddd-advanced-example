using AuthorizationOperation.Domain.User.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace AuthorizationOperation.Infrastructure.EF.Config
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");

            builder.Property(e => e.Guid).HasConversion<Guid>().IsRequired();
            builder.Property(e => e.UserName).HasColumnName("username").IsRequired();
            builder.Property(e => e.Password).IsRequired();
            builder.Property(e => e.Email).IsRequired();
            builder.Property(e => e.Created).IsRequired();

            builder.HasKey("Guid");
        }
    }
}
