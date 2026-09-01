using System.ComponentModel.DataAnnotations;

namespace OctaPro.DTO.Request;

public class UserPermissionRequest
{
    [Required]
    public int PermissionId { get; set; }
}
