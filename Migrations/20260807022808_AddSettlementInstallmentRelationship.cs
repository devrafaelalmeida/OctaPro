using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OctaPro.Migrations
{
    /// <inheritdoc />
    public partial class AddSettlementInstallmentRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "settlement_id",
                table: "settlement_installments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_settlement_installments_settlement_id",
                table: "settlement_installments",
                column: "settlement_id");

            migrationBuilder.AddForeignKey(
                name: "FK_settlement_installments_settlement_settlement_id",
                table: "settlement_installments",
                column: "settlement_id",
                principalTable: "settlement",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_settlement_installments_settlement_settlement_id",
                table: "settlement_installments");

            migrationBuilder.DropIndex(
                name: "IX_settlement_installments_settlement_id",
                table: "settlement_installments");

            migrationBuilder.DropColumn(
                name: "settlement_id",
                table: "settlement_installments");
        }
    }
}
