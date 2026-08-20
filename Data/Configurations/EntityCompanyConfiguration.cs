using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OctaPro.Models;

namespace OctaPro.Data.Configurations;

public class EntityCompanyConfiguration : IEntityTypeConfiguration<EntityCompany>
{
    public void Configure(EntityTypeBuilder<EntityCompany> entity)
    {
        entity.HasKey(e => e.Id)
              .HasName("entities_company_pkey");

        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()");

        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()");

        entity.HasOne(d => d.Entity)
            .WithOne(p => p.EntityCompany)
            .HasForeignKey<EntityCompany>(d => d.EntityId)
            .HasConstraintName("fk_entity_company");
    }
}
