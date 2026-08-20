using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OctaPro.Models;

namespace OctaPro.Data.Configurations;

public class CorporationConfiguration : IEntityTypeConfiguration<Corporation>
{
    public void Configure(EntityTypeBuilder<Corporation> entity)
    {
        entity.HasKey(e => e.Id)
            .HasName("corporations_pkey");

        entity.Property(e => e.Id)
            .UseIdentityAlwaysColumn();

        entity.Property(e => e.IdPublic)
            .HasDefaultValueSql("gen_random_uuid()");

        entity.Property(e => e.IsActive)
            .HasDefaultValue(true);

        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()");

        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()");
    }
}
