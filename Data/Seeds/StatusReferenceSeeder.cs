using Microsoft.EntityFrameworkCore;
using OctaPro.Enums;
using OctaPro.Models;

namespace OctaPro.Data.Seeds;

public static class StatusReferenceSeeder
{
    public static async Task SeedStatusReferencesAsync(AppDbContext context)
    {
        var now = DateTime.UtcNow;
        var existingStatusReferences = await context.StatusReferences
            .ToDictionaryAsync(statusReference => statusReference.Id);

        foreach (var statusReference in GetStatusReferences(now))
        {
            if (existingStatusReferences.TryGetValue(statusReference.Id, out var existingStatusReference))
            {
                existingStatusReference.Description = statusReference.Description;
                existingStatusReference.UpdatedAt = now;
                continue;
            }

            await context.Database.ExecuteSqlInterpolatedAsync($"""
                INSERT INTO status_reference (
                    id,
                    description,
                    created_at,
                    updated_at
                )
                OVERRIDING SYSTEM VALUE
                VALUES (
                    {statusReference.Id},
                    {statusReference.Description},
                    {statusReference.CreatedAt},
                    {statusReference.UpdatedAt}
                );
                """);
        }

        await context.SaveChangesAsync();
    }

    private static IEnumerable<StatusReference> GetStatusReferences(DateTime now)
    {
        return Enum.GetValues<StatusReferenceEnum>()
            .Select(statusReference => new StatusReference
            {
                Id = (int)statusReference,
                Description = statusReference.ToString(),
                CreatedAt = now,
                UpdatedAt = now
            });
    }
}
