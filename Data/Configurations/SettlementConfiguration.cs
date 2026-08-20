using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OctaPro.Models;

namespace OctaPro.Data.Configurations;

public class SettlementConfiguration : IEntityTypeConfiguration<Settlement>
{
    public void Configure(EntityTypeBuilder<Settlement> entity)
    {
        entity.HasKey(e => e.Id)
              .HasName("installments_settlement_pkey");

        entity.Property(e => e.Id)
            .UseIdentityAlwaysColumn();

        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()");

        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()");

        entity.HasOne(d => d.JudicialProcess)
            .WithMany(p => p.Settlements)
            .HasForeignKey(d => d.JudicialProcessId)
            .HasConstraintName("fk_judicial_process");

        entity.HasOne(d => d.User)
            .WithMany(p => p.Settlements)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("fk_user_id");

        entity.Ignore(e => e.SettlementInstallments);
    }
}
