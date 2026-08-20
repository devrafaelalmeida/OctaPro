using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OctaPro.Migrations
{
    /// <inheritdoc />
    public partial class RenameInstallmentReferenceId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_installments_settlement_referency_id",
                table: "installments");

            migrationBuilder.DropIndex(
                name: "IX_installments_referency_id",
                table: "installments");

            migrationBuilder.RenameColumn(
                name: "referency_id",
                table: "installments",
                newName: "reference_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "reference_id",
                table: "installments",
                newName: "referency_id");

            migrationBuilder.CreateIndex(
                name: "IX_installments_referency_id",
                table: "installments",
                column: "referency_id");

            migrationBuilder.AddForeignKey(
                name: "FK_installments_settlement_referency_id",
                table: "installments",
                column: "referency_id",
                principalTable: "settlement",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
