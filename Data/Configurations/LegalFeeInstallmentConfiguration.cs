using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OctaPro.Models;

namespace OctaPro.Data.Configurations;

public class LegalFeeInstallmentConfiguration : IEntityTypeConfiguration<LegalFeeInstallment>
{
    public void Configure(EntityTypeBuilder<LegalFeeInstallment> entity)
    {
        entity.Ignore(e => e.LegalFee);
    }
}
