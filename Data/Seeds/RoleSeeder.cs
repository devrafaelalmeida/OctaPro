using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OctaPro.Enums;

namespace OctaPro.Data.Seeds
{
    public static class RoleSeeder
{
    public static async Task SeedRolesAsync(RoleManager<IdentityRole<long>> roleManager)
    {
        UserRole[] roles = new[] { UserRole.ADMIN, UserRole.COMMON, UserRole.MANAGER };

        foreach (var role in roles)
        {
            var roleName = GetRoleName(role);

            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole<long>
                {
                    Id = (long)role,
                    Name = roleName,
                    NormalizedName = roleName.ToUpper()
                });
            }
        }
    }

    private static string GetRoleName(UserRole role)
    {
        return role switch
        {
            UserRole.ADMIN => "Admin",
            UserRole.COMMON => "Common",
            UserRole.MANAGER => "Manager",
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };
    }
}
}
