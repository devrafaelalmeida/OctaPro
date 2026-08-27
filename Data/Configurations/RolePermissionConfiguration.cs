using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OctaPro.Models;

namespace OctaPro.Data.Configurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> entity)
    {
        entity.HasKey(e => e.Id)
            .HasName("permissions_role_pkey");

        entity.HasIndex(e => new { e.RoleId, e.PermissionId })
            .IsUnique()
            .HasDatabaseName("permissions_role_role_id_permission_id_unique");

        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()");

        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()");

        entity.HasOne(e => e.Role)
            .WithMany()
            .HasForeignKey(e => e.RoleId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_permissions_role_role");

        entity.HasOne(e => e.Permission)
            .WithMany(e => e.RolePermissions)
            .HasForeignKey(e => e.PermissionId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_permissions_role_permission");
    }
}
