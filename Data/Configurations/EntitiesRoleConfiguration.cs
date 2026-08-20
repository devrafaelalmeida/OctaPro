using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OctaPro.Models;

namespace OctaPro.Data.Configurations;

public class EntitiesRoleConfiguration : IEntityTypeConfiguration<EntitiesRole>
{
    public void Configure(EntityTypeBuilder<EntitiesRole> entity)
    {
        entity.HasKey(e => e.Id)
              .HasName("entities_roles_pkey");

        entity.Property(e => e.Id)
            .UseIdentityAlwaysColumn();

        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()");

        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()");
    }
}
