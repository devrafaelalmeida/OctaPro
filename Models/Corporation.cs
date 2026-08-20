using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OctaPro.Models;

[Index(nameof(IdPublic), IsUnique = true, Name = "corporations_id_public_key")]
[Table("corporations")]
public class Corporation
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Column("id_public")]
    public Guid IdPublic { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("legal_name")]
    public string LegalName { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("trade_name")]
    public string TradeName { get; set; } = string.Empty;

    [Required]
    [MaxLength(14)]
    [Column("cnpj")]
    public string Cnpj { get; set; } = string.Empty;

    [Required]
    [Column("opening_date")]
    public DateOnly OpeningDate { get; set; }

    [MaxLength(50)]
    [Column("state_registration")]
    public string? StateRegistration { get; set; }

    [MaxLength(50)]
    [Column("municipal_registration")]
    public string? MunicipalRegistration { get; set; }

    [MaxLength(50)]
    [Column("tax_regime")]
    public string? TaxRegime { get; set; }

    [Required]
    [MaxLength(8)]
    [Column("zip_code")]
    public string ZipCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("street")]
    public string Street { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    [Column("number")]
    public string Number { get; set; } = string.Empty;

    [MaxLength(200)]
    [Column("complement")]
    public string? Complement { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("district")]
    public string District { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("city")]
    public string City { get; set; } = string.Empty;

    [Required]
    [MaxLength(2)]
    [Column("state")]
    public string State { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [MaxLength(50)]
    [Column("mobile")]
    public string? Mobile { get; set; }

    [MaxLength(50)]
    [Column("phone")]
    public string? Phone { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at", TypeName = "timestamp with time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp with time zone")]
    public DateTime UpdatedAt { get; set; }

    public Corporation()
    {
        IdPublic = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        IsActive = true;
    }
}
