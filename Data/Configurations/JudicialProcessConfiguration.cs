using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OctaPro.Models;

namespace OctaPro.Data.Configurations;

public class JudicialProcessConfiguration : IEntityTypeConfiguration<JudicialProcess>
{
    public void Configure(EntityTypeBuilder<JudicialProcess> entity)
    {
        entity.HasKey(e => e.Id)
              .HasName("judicial_processes_pkey");

        entity.Property(e => e.Id)
            .UseIdentityAlwaysColumn();

        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()");

        entity.Property(e => e.IsArchived)
            .HasDefaultValue(false);

        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()");

        entity.HasOne(d => d.NatureAction)
            .WithMany(p => p.JudicialProcesses)
            .HasForeignKey(d => d.NatureActionId)
            .HasConstraintName("fk_nature_action");

        // entity.HasOne(d => d.User)
        //     .WithMany(p => p.JudicialProcesses)
        //     .HasForeignKey(d => d.UserId)
        //     .HasConstraintName("fk_user_id");
    }
}
