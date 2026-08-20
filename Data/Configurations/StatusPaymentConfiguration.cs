using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OctaPro.Models;

namespace OctaPro.Data.Configurations;

public class StatusPaymentConfiguration : IEntityTypeConfiguration<StatusPayment>
{
    public void Configure(EntityTypeBuilder<StatusPayment> entity)
    {
        entity.HasKey(e => e.Id)
              .HasName("status_payment_pkey");

        entity.Property(e => e.Id)
            .UseIdentityAlwaysColumn();

        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()");

        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()");
    }
}
