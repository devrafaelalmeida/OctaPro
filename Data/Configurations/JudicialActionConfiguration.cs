using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OctaPro.Models;

namespace OctaPro.Data.Configurations;

public class JudicialActionConfiguration : IEntityTypeConfiguration<JudicialAction>
{
    public void Configure(EntityTypeBuilder<JudicialAction> entity)
    {
        entity.HasKey(e => e.Id)
              .HasName("judicials_actions_pkey");

        entity.Property(e => e.Id)
            .UseIdentityAlwaysColumn();

        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()");

        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()");

        entity.HasOne(d => d.NatureAction)
            .WithMany(p => p.JudicialAction)
            .HasForeignKey(d => d.NatureActionId)
            .HasConstraintName("fk_nature_action");
    }
}
