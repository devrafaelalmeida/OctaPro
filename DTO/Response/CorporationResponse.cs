namespace OctaPro.DTO.Response;

public class CorporationResponse
{
    public Guid IdPublic { get; set; }
    public string? LegalName { get; set; }
    public string? TradeName { get; set; }
    public string? Cnpj { get; set; }
    public DateOnly OpeningDate { get; set; }
    public string? StateRegistration { get; set; }
    public string? MunicipalRegistration { get; set; }
    public string? TaxRegime { get; set; }
    public string? ZipCode { get; set; }
    public string? Street { get; set; }
    public string? Number { get; set; }
    public string? Complement { get; set; }
    public string? District { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Email { get; set; }
    public string? Mobile { get; set; }
    public string? Phone { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
