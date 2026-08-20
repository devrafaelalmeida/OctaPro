using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OctaPro.Data;

#nullable disable

namespace OctaPro.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260809120500_AddInstallmentsTypeInstallmentsFk")]
    public partial class AddInstallmentsTypeInstallmentsFk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                INSERT INTO type_installments (id, description)
                VALUES (1, 'Acordo'), (2, 'Honorário')
                ON CONFLICT (id) DO UPDATE
                SET description = EXCLUDED.description;
                """);

            migrationBuilder.Sql("""
                UPDATE installments
                SET type_id = 1
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM type_installments
                    WHERE type_installments.id = installments.type_id
                );
                """);

            migrationBuilder.AddForeignKey(
                name: "FK_installments_type_installments_type_id",
                table: "installments",
                column: "type_id",
                principalTable: "type_installments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_installments_type_installments_type_id",
                table: "installments");
        }
    }
}
