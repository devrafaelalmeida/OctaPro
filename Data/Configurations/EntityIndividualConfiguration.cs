using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OctaPro.Models;

namespace OctaPro.Data.Configurations;

public class EntityIndividualConfiguration : IEntityTypeConfiguration<EntityIndividual>
{
    public void Configure(EntityTypeBuilder<EntityIndividual> entity)
    {
        entity.HasKey(e => e.Id)
              .HasName("entities_individual_pkey");

        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()");

        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()");

        entity.HasOne(d => d.Entity)
            .WithOne(p => p.EntityIndividual)
            .HasForeignKey<EntityIndividual>(d => d.EntityId)
            .HasConstraintName("fk_entity_individual");
    }
}
