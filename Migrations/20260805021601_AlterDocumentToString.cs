using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OctaPro.Migrations
{
    /// <inheritdoc />
    public partial class AlterDocumentToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
   
            // migrationBuilder.RenameTable(
            //     name: "SettlementInstallments",
            //     newName: "settlement_installments");

            // migrationBuilder.AddPrimaryKey(
            //     name: "PK_settlement_installments",
            //     table: "settlement_installments",
            //     column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropPrimaryKey(
            //     name: "PK_settlement_installments",
            //     table: "settlement_installments");

            // migrationBuilder.RenameTable(
            //     name: "settlement_installments",
            //     newName: "SettlementInstallments");

        }
    }
}
