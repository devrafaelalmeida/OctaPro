using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OctaPro.Models;

namespace OctaPro.Data.Configurations;

public class SettlementInstallmentConfiguration : IEntityTypeConfiguration<SettlementInstallment>
{
    public void Configure(EntityTypeBuilder<SettlementInstallment> entity)
    {
        entity.Property(e => e.SettlementId)
            .HasColumnName("settlement_id");

        entity.Property(e => e.PaidAmount)
            .HasPrecision(10, 2)
            .HasColumnName("paid_amount");

        entity.HasOne(e => e.Settlement)
            .WithMany(e => e.SettlementInstallments)
            .HasForeignKey(e => e.SettlementId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
