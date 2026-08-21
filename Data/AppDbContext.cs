using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OctaPro.Models;

namespace OctaPro.Data;

public partial class AppDbContext : IdentityDbContext<User, IdentityRole<long>, long>
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cache> Caches { get; set; }
    
    public virtual DbSet<CacheLock> CacheLocks { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Corporation> Corporations { get; set; }

    public virtual DbSet<EntityCompany> EntitiesCompanies { get; set; }

    public virtual DbSet<EntityIndividual> EntitiesIndividuals { get; set; }

    public virtual DbSet<EntitiesRole> EntitiesRoles { get; set; }

    public virtual DbSet<EntitiesRolesMap> EntitiesRolesMaps { get; set; }

    public virtual DbSet<Entity> Entities { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<JobBatch> JobBatches { get; set; }

    public virtual DbSet<JudicialProcess> JudicialProcesses { get; set; }

    public virtual DbSet<JudicialProcessUser> JudicialProcessUsers { get; set; }

    public virtual DbSet<JudicialAction> JudicialActions { get; set; }

    public virtual DbSet<LegalFee> LegalFees { get; set; }

    public virtual DbSet<Installment> Installments { get; set; }

    public virtual DbSet<LegalFeeInstallment> LegalFeeInstallments { get; set; }

    public virtual DbSet<Migration> Migrations { get; set; }

    public virtual DbSet<NatureAction> NatureActions { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<Settlement> Settlements { get; set; }

    public DbSet<SettlementInstallment> SettlementInstallments { get; set; }

    public virtual DbSet<StatusEntity> StatusEntities { get; set; }

    public virtual DbSet<StatusPayment> StatusPayments { get; set; }

    public virtual DbSet<StatusReference> StatusReferences { get; set; }

    public virtual DbSet<TypeInstallment> TypeInstallments { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        

        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<IdentityRole<long>>().ToTable("roles");
        modelBuilder.Entity<IdentityUserRole<long>>().ToTable("user_roles");

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entity.GetProperties())
            {
                if (property.ClrType == typeof(DateTime) ||
                    property.ClrType == typeof(DateTime?))
                {
                    property.SetColumnType("timestamptz");
                }
            }
        }
    }

}
