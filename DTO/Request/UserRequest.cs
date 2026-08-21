using System.ComponentModel.DataAnnotations;

namespace OctaPro.DTO.Request;

public class UserRequest
{
    [Required]
    [MaxLength(256)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [MaxLength(11)]
    public string CPF { get; set; } = string.Empty;

    [Required]
    public DateOnly BirthDate { get; set; }

    [Required]
    [MaxLength(50)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    public string? Password { get; set; }

    [Required]
    public int CorporationId { get; set; }

    [Required]
    public int RoleId { get; set; }

    [Required]
    [MaxLength(8)]
    public string CEP { get; set; } = string.Empty;

    [Required]
    [MaxLength(2)]
    public string UF { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string City { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Address { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string NumberHouse { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Complement { get; set; }

    [Required]
    [MaxLength(255)]
    public string Neithborhood { get; set; } = string.Empty;
}
