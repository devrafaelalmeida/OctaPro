using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OctaPro.Models;

namespace OctaPro.Data.Configurations;

public class NatureActionConfiguration : IEntityTypeConfiguration<NatureAction>
{
    public void Configure(EntityTypeBuilder<NatureAction> entity)
    {
        entity.HasKey(e => e.Id)
              .HasName("nature_actions_pkey");

        entity.Property(e => e.Id)
            .UseIdentityAlwaysColumn();

        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()");

        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()");
    }
}
