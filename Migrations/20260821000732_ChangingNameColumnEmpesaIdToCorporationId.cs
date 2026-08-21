using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OctaPro.Migrations
{
    /// <inheritdoc />
    public partial class ChangingNameColumnEmpesaIdToCorporationId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "corporation_id",
                table: "users",
                newName: "corporation_id");

            migrationBuilder.RenameColumn(
                name: "corporation_id",
                table: "settlement",
                newName: "corporation_id");

            migrationBuilder.RenameColumn(
                name: "corporation_id",
                table: "legal_fees",
                newName: "corporation_id");

            migrationBuilder.RenameColumn(
                name: "corporation_id",
                table: "judicial_processes",
                newName: "corporation_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "corporation_id",
                table: "users",
                newName: "corporation_id");

            migrationBuilder.RenameColumn(
                name: "corporation_id",
                table: "settlement",
                newName: "corporation_id");

            migrationBuilder.RenameColumn(
                name: "corporation_id",
                table: "legal_fees",
                newName: "corporation_id");

            migrationBuilder.RenameColumn(
                name: "corporation_id",
                table: "judicial_processes",
                newName: "corporation_id");
        }
    }
}
