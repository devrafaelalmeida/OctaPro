using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OctaPro.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnEmpresaIdToManyTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "empresa_id",
                table: "settlement",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "empresa_id",
                table: "legal_fees",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "empresa_id",
                table: "judicial_processes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "settlement");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "legal_fees");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "judicial_processes");
        }
    }
}
