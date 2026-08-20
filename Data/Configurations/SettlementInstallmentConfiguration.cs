using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OctaPro.Models;

namespace OctaPro.Data.Configurations;

public class SettlementInstallmentConfiguration : IEntityTypeConfiguration<SettlementInstallment>
{
    public void Configure(EntityTypeBuilder<SettlementInstallment> entity)
    {
        entity.Ignore(e => e.Settlement);
    }
}
