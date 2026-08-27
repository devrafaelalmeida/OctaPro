using Microsoft.EntityFrameworkCore;

namespace OctaPro.Data.Seeds;

public static class CorporationSeeder
{
    public static async Task SeedInitialCorporationAsync(AppDbContext context)
    {
        const long initialCorporationId = 1;

        var exists = await context.Corporations
            .AnyAsync(corporation => corporation.Id == initialCorporationId);

        if (exists)
            return;

        await context.Database.ExecuteSqlRawAsync("""
            INSERT INTO corporations (
                id,
                id_public,
                legal_name,
                trade_name,
                cnpj,
                opening_date,
                zip_code,
                street,
                number,
                district,
                city,
                state,
                email,
                is_active,
                created_at,
                updated_at
            )
            OVERRIDING SYSTEM VALUE
            VALUES (
                1,
                gen_random_uuid(),
                'EMPRESA INICIAL',
                'EMPRESA INICIAL',
                '00000000000000',
                CURRENT_DATE,
                '00000000',
                'NAO INFORMADO',
                'S/N',
                'NAO INFORMADO',
                'NAO INFORMADO',
                'BA',
                'empresa.inicial@octapro.com',
                TRUE,
                now(),
                now()
            );
            """);
    }
}
