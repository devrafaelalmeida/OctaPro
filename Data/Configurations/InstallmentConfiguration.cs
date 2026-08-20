using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OctaPro.Models;

namespace OctaPro.Data.Configurations;

public class InstallmentConfiguration : IEntityTypeConfiguration<Installment>
{
    public void Configure(EntityTypeBuilder<Installment> entity)
    {
        entity.HasKey(e => e.Id)
            .HasName("PK_installments");

        entity.HasDiscriminator<string>("Discriminator")
            .HasValue<SettlementInstallment>("Settlement")
            .HasValue<LegalFeeInstallment>("LegalFee");

        entity.HasOne(e => e.TypeInstallment)
            .WithMany(e => e.Installments)
            .HasForeignKey(e => e.TypeId)
            .HasConstraintName("FK_installments_type_installments_type_id");
    }
}
