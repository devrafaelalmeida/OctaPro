using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OctaPro.Migrations
{
    /// <inheritdoc />
    public partial class FixSettlementInstallmentDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Document",
                table: "settlement_installments",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Document",
                table: "settlement_installments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)"
            );
        }
    }
}
