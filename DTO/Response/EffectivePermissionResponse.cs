namespace OctaPro.DTO.Response;

public class EffectivePermissionResponse : PermissionResponse
{
    public bool FromRole { get; set; }
    public bool Direct { get; set; }
    public List<string> Roles { get; set; } = new();
}
