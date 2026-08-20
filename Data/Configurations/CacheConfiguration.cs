using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OctaPro.Models;

namespace OctaPro.Data.Configurations;

public class CacheConfiguration : IEntityTypeConfiguration<Cache>
{
    public void Configure(EntityTypeBuilder<Cache> entity)
    {
        entity.HasKey(e => e.Key)
              .HasName("cache_pkey");
    }
}
