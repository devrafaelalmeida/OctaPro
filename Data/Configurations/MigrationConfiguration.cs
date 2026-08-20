using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OctaPro.Models;

namespace OctaPro.Data.Configurations;

public class MigrationConfiguration : IEntityTypeConfiguration<Migration>
{
    public void Configure(EntityTypeBuilder<Migration> entity)
    {
        entity.HasKey(e => e.Id)
              .HasName("migrations_pkey");
    }
}
