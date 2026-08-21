namespace OctaPro.DTO.Response;

public class UserResponse
{
    public Guid IdPublic { get; set; }
    public string? UserName { get; set; }
    public string? CPF { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public int CorporationId { get; set; }
    public long? RoleId { get; set; }
    public string? CEP { get; set; }
    public string? UF { get; set; }
    public string? City { get; set; }
    public string? Address { get; set; }
    public string? NumberHouse { get; set; }
    public string? Complement { get; set; }
    public string? Neithborhood { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
