using Microsoft.AspNetCore.Authorization;
using OctaPro.Services.interfaces;

namespace OctaPro.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IAccessControlService _accessControlService;
    private readonly ICurrentUserService _currentUserService;

    public PermissionAuthorizationHandler(
        IAccessControlService accessControlService,
        ICurrentUserService currentUserService)
    {
        _accessControlService = accessControlService;
        _currentUserService = currentUserService;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var user = await _currentUserService.GetCurrentUserAsync();

        if (user == null)
            return;

        if (await _accessControlService.HasPermissionAsync(user, requirement.Permission))
            context.Succeed(requirement);
    }
}
