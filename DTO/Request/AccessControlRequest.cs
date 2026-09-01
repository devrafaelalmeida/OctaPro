using System.ComponentModel.DataAnnotations;

namespace OctaPro.DTO.Request;

public class AccessControlRequest
{
    [Required]
    public List<int> RolesPermissions { get; set; } = new();

    [Required]
    public List<int> ExtrasPermissions { get; set; } = new();
}
