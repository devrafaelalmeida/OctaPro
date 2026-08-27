using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OctaPro.Models;

namespace OctaPro.Data.Configurations;

public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
{
    public void Configure(EntityTypeBuilder<UserPermission> entity)
    {
        entity.HasKey(e => e.Id)
            .HasName("users_permissions_pkey");

        entity.HasIndex(e => new { e.UserId, e.PermissionId })
            .IsUnique()
            .HasDatabaseName("users_permissions_user_id_permission_id_unique");

        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()");

        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()");

        entity.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_users_permissions_user");

        entity.HasOne(e => e.Permission)
            .WithMany(e => e.UserPermissions)
            .HasForeignKey(e => e.PermissionId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_users_permissions_permission");
    }
}
