using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OctaPro.Models;

namespace OctaPro.Data.Configurations;

public class JudicialProcessEntityConfiguration : IEntityTypeConfiguration<JudicialProcessEntity>
{
    public void Configure(EntityTypeBuilder<JudicialProcessEntity> entity)
    {
        entity.HasKey(e => new { e.JudicialProcessId, e.EntityId });

        entity.HasOne(e => e.JudicialProcess)
            .WithMany(p => p.JudicialProcessEntities)
            .HasForeignKey(e => e.JudicialProcessId)
            .HasConstraintName("fk_judicial_process_entity_process");

        entity.HasOne(e => e.Entity)
            .WithMany(e => e.JudicialProcessEntities)
            .HasForeignKey(e => e.EntityId)
            .HasConstraintName("fk_judicial_process_entity_entity");
    }
}
