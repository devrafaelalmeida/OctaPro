namespace OctaPro.DTO.Response;

public class TenantDto
{
    public string Domain { get; set; } = string.Empty;

    public string ConnectionName { get; set; } = string.Empty;

    public string DataSource { get; set; } = string.Empty;

    public string Database { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
