using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OctaPro.Models;

namespace OctaPro.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.HasKey(e => e.Id)
              .HasName("users_pkey");

        entity.Property(e => e.Id)
            .HasColumnName("id");

        entity.Property(e => e.Email)
            .HasMaxLength(255)
            .HasColumnName("email");    
    }
}
