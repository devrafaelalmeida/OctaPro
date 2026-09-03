namespace OctaPro.Models;

public class Tenant
{
    public int Id { get; set; }

    public string Domain { get; set; } = string.Empty;

    public string ConnectionName { get; set; } = string.Empty;

    public string DataSource { get; set; } = string.Empty;

    public string Database { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public bool Ativo { get; set; } = true;

    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}
