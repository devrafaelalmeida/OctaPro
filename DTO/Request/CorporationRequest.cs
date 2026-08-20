using System.ComponentModel.DataAnnotations;

namespace OctaPro.DTO.Request;

public class CorporationRequest
{
    [Required]
    [MaxLength(255)]
    public string LegalName { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string TradeName { get; set; } = string.Empty;

    [Required]
    [MaxLength(14)]
    public string Cnpj { get; set; } = string.Empty;

    [Required]
    public DateOnly OpeningDate { get; set; }

    [MaxLength(50)]
    public string? StateRegistration { get; set; }

    [MaxLength(50)]
    public string? MunicipalRegistration { get; set; }

    [MaxLength(50)]
    public string? TaxRegime { get; set; }

    [Required]
    [MaxLength(8)]
    public string ZipCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Street { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Number { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Complement { get; set; }

    [Required]
    [MaxLength(255)]
    public string District { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string City { get; set; } = string.Empty;

    [Required]
    [MaxLength(2)]
    public string State { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Mobile { get; set; }

    [MaxLength(50)]
    public string? Phone { get; set; }

    public bool IsActive { get; set; } = true;
}
