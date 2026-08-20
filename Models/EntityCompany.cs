using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OctaPro.Models;

[Table("entities_company")]
public partial class EntityCompany
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("entity_id")]
    public long EntityId { get; set; }

    [MaxLength(20)]
    [Column("cnpj")]
    public string? Cnpj { get; set; }

    [MaxLength(200)]
    [Column("corporate_name")]
    public string? CorporateName { get; set; }

    [MaxLength(200)]
    [Column("trade_name")]
    public string? TradeName { get; set; }

    [MaxLength(255)]
    [Column("email")]
    public string? CorporateEmail { get; set; }

    [MaxLength(50)]
    [Column("mobile")]
    public string? CorporateMobile { get; set; }

    [MaxLength(50)]
    [Column("phone")]
    public string? CorporatePhone { get; set; }

    [Column("created_at", TypeName = "timestamp with time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp with time zone")]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey(nameof(EntityId))]
    public virtual Entity Entity { get; set; } = null!;
    [Column("address")]
    public string? Address { get; set; }

    [StringLength(8)]
    [Column("cep")]
    public string? Cep { get; set; }
    [Column("house_number")]
    public string? HouseNumber { get; set; }
    [StringLength(200)]
    [Column("complement")]
    public string? Complement { get; set; }
    [Column("city")]
    public string? City { get; set; }
    [Column("district")]
    public string? District { get; set; }
    
    [StringLength(2)]
    [Column("uf")]

    public string? Uf { get; set; }
}
