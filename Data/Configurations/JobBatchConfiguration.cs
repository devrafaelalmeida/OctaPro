using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OctaPro.Models;

namespace OctaPro.Data.Configurations;

public class JobBatchConfiguration : IEntityTypeConfiguration<JobBatch>
{
    public void Configure(EntityTypeBuilder<JobBatch> entity)
    {
        entity.HasKey(e => e.Id)
              .HasName("job_batches_pkey");
    }
}
