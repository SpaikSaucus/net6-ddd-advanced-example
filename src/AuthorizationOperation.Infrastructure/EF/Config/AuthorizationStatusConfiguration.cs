using AuthorizationOperation.Domain.Authorization.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;

namespace AuthorizationOperation.Infrastructure.EF.Config
{
    public class AuthorizationStatusConfiguration : IEntityTypeConfiguration<AuthorizationStatus>
    {
        public void Configure(EntityTypeBuilder<AuthorizationStatus> builder)
        {
            builder.ToTable("authorization_status");
            builder.Property(e => e.Id).IsRequired().HasConversion<int>();
            builder.Property(e => e.Name).IsRequired();

            builder.HasData(Enum.GetValues(typeof(AuthorizationStatusEnum))
                        .Cast<AuthorizationStatusEnum>()
                        .Select(e => new AuthorizationStatus()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
                    );
        }
    }
}
