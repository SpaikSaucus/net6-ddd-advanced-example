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

            builder.Property(e => e.UUID).HasColumnName("uuid").HasConversion<Guid>().IsRequired();
            builder.Property(e => e.UserName).HasColumnName("user_name").IsRequired();
            builder.Property(e => e.Password).HasColumnName("password").IsRequired();
            builder.Property(e => e.Email).HasColumnName("email").IsRequired();
            builder.Property(e => e.Created).HasColumnName("created").IsRequired();

            builder.HasKey(e => e.UUID).HasName("pk_user_uuid");
        }
    }
}
