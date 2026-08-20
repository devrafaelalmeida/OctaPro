using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OctaPro.Models;

namespace OctaPro.Data.Configurations;

public class TypeInstallmentConfiguration : IEntityTypeConfiguration<TypeInstallment>
{
    public void Configure(EntityTypeBuilder<TypeInstallment> entity)
    {
        entity.HasKey(e => e.Id)
            .HasName("PK_type_installments");

        entity.Property(e => e.Id)
            .ValueGeneratedNever();

        entity.HasData(
            new TypeInstallment { Id = Installment.SettlementTypeId, Description = "Acordo" },
            new TypeInstallment { Id = Installment.LegalFeeTypeId, Description = "Honorário" }
        );
    }
}
