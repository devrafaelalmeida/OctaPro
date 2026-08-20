using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OctaPro.Models;

namespace OctaPro.Data.Configurations;

public class LegalFeeEntityConfiguration : IEntityTypeConfiguration<LegalFeeEntity>
{
    public void Configure(EntityTypeBuilder<LegalFeeEntity> entity)
    {
        entity.HasKey(e => new { e.LegalFeeId, e.EntityId });

        entity.HasOne(e => e.Entity)
            .WithMany(e => e.LegalFeeEntities)
            .HasForeignKey(e => e.EntityId)
            .HasConstraintName("fk_legal_fee_entity_entity");

        entity.HasOne(e => e.LegalFee)
            .WithMany(lf => lf.LegalFeeEntities)
            .HasForeignKey(e => e.LegalFeeId)
            .HasConstraintName("fk_legal_fee_entity_legal_fee");
    }
}
