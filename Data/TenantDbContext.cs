using Microsoft.EntityFrameworkCore;
using OctaPro.Models;

namespace OctaPro.Data;

public class TenantDbContext : DbContext
{
    public TenantDbContext(DbContextOptions<TenantDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tenant> Tenants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.ToTable("tenants");

            entity.HasKey(tenant => tenant.Id);

            entity.HasIndex(tenant => tenant.Domain)
                .IsUnique();

            entity.Property(tenant => tenant.Id)
                .HasColumnName("id");

            entity.Property(tenant => tenant.Domain)
                .HasColumnName("domain")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(tenant => tenant.ConnectionName)
                .HasColumnName("connection_name")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(tenant => tenant.DataSource)
                .HasColumnName("data_source")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(tenant => tenant.Database)
                .HasColumnName("database")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(tenant => tenant.Username)
                .HasColumnName("username")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(tenant => tenant.Password)
                .HasColumnName("password")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(tenant => tenant.Ativo)
                .HasColumnName("ativo")
                .HasDefaultValue(true)
                .IsRequired();

            entity.Property(tenant => tenant.CriadoEm)
                .HasColumnName("criado_em")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();
        });
    }
}
