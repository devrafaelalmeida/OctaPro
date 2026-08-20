using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OctaPro.Models;

namespace OctaPro.Data.Configurations;

public class LegalFeeConfiguration : IEntityTypeConfiguration<LegalFee>
{
    public void Configure(EntityTypeBuilder<LegalFee> entity)
    {
        entity.HasKey(e => e.Id)
              .HasName("legal_fees_pkey");

        entity.Property(e => e.Id)
            .UseIdentityAlwaysColumn();

        entity.Property(e => e.Amount)
            .HasDefaultValue(0.0m);

        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()");

        entity.Property(e => e.QuantityInstallment)
            .HasDefaultValue(1);

        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()");

        entity.HasOne(d => d.JudicialProcess)
            .WithMany(p => p.LegalFees)
            .HasForeignKey(d => d.JudicialProcessId)
            .HasConstraintName("fk_judicial_process");

        // entity.HasOne(d => d.StatusPayment)
        //     .WithMany(p => p.LegalFees)
        //     .HasForeignKey(d => d.StatusPaymentId)
        //     .HasConstraintName("fk_status_payment");

        entity.HasOne(d => d.User)
            .WithMany(p => p.LegalFees)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("fk_user_id");

        entity.Ignore(e => e.LegalFeeInstallments);
    }
}
