using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OctaPro.Models;

namespace OctaPro.Data.Configurations;

public class JudicialProcessUserConfiguration : IEntityTypeConfiguration<JudicialProcessUser>
{
    public void Configure(EntityTypeBuilder<JudicialProcessUser> entity)
    {
        entity.HasKey(e => e.Id)
              .HasName("judicial_process_user_pkey");

        entity.HasIndex(e => new { e.JudicialProcessId, e.UserId })
              .IsUnique()
              .HasDatabaseName("unique_process_user");

        entity.Property(e => e.AccessLevel)
            .HasDefaultValueSql("'private'::character varying");

        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()");

        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()");

        entity.HasOne(d => d.User)
            .WithMany(p => p.JudicialProcessUsers)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("fk_user");
    }
}
