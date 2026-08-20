using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OctaPro.Models;

namespace OctaPro.Data.Configurations;

public class EntitiesRolesMapConfiguration : IEntityTypeConfiguration<EntitiesRolesMap>
{
    public void Configure(EntityTypeBuilder<EntitiesRolesMap> entity)
    {
        entity.HasKey(e => e.Id)
              .HasName("entities_roles_map_pkey");

        entity.HasIndex(e => new { e.EntityId, e.RoleId })
              .IsUnique()
              .HasDatabaseName("unique_entity_role");

        entity.Property(e => e.AssignedAt)
            .HasDefaultValueSql("now()");

        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()");

        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()");

        entity.HasOne(d => d.Role)
            .WithMany(p => p.EntitiesRolesMaps)
            .HasForeignKey(d => d.RoleId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_entities_roles_map");
    }
}
