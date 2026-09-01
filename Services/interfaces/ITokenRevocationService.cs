namespace OctaPro.Services.interfaces;

public interface ITokenRevocationService
{
    Task RevokeCurrentTokenAsync();
    Task<bool> IsTokenRevokedAsync(string token);
}
