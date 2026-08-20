using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OctaPro.Models;

namespace OctaPro.Data.Configurations;

public class CacheLockConfiguration : IEntityTypeConfiguration<CacheLock>
{
    public void Configure(EntityTypeBuilder<CacheLock> entity)
    {
        entity.HasKey(e => e.Key)
              .HasName("cache_locks_pkey");
    }
}
