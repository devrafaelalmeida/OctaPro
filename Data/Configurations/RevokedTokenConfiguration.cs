using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OctaPro.Models;

namespace OctaPro.Data.Configurations;

public class RevokedTokenConfiguration : IEntityTypeConfiguration<RevokedToken>
{
    public void Configure(EntityTypeBuilder<RevokedToken> entity)
    {
        entity.HasKey(e => e.Id)
            .HasName("revoked_tokens_pkey");

        entity.HasIndex(e => e.TokenHash)
            .IsUnique()
            .HasDatabaseName("revoked_tokens_token_hash_unique");

        entity.HasIndex(e => e.ExpiresAt)
            .HasDatabaseName("revoked_tokens_expires_at_index");

        entity.Property(e => e.RevokedAt)
            .HasDefaultValueSql("now()");
    }
}
