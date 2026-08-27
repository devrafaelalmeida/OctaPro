using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OctaPro.Models;

[Index(nameof(IdPublic), IsUnique = true, Name = "entities_id_public_key")]
[Table("entities")]
public partial class Entity
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [MaxLength(2)]
    [Column("entity_type")]
    public string EntityType { get; set; } = null!;

    [Column("status_id")]
    public int StatusId { get; set; }

    [Column("created_at", TypeName = "timestamp with time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp with time zone")]
    public DateTime UpdatedAt { get; set; }

    [Column("id_public")]
    public Guid IdPublic { get; set; }

    [Column("corporation_id")]
    public long CorporationId { get; set; }

    public virtual EntityIndividual? EntityIndividual { get; set; }
    public virtual EntityCompany? EntityCompany { get; set; }

    [ForeignKey(nameof(CorporationId))]
    public virtual Corporation Corporation { get; set; } = null!;

    [NotMapped]
    public virtual ICollection<LegalFeeInstallment> LegalFeeInstallments { get; set; } = new List<LegalFeeInstallment>();

    public ICollection<JudicialProcessEntity> JudicialProcessEntities { get; set; } = new List<JudicialProcessEntity>();

    public ICollection<LegalFeeEntity> LegalFeeEntities { get; set; } = new List<LegalFeeEntity>();
    
    public Entity()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    
    }
