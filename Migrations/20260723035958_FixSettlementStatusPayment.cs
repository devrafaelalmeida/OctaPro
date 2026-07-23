using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OctaPro.Migrations
{
    /// <inheritdoc />
    public partial class FixSettlementStatusPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropColumn(
            //     name: "StatusPaymentEnum",
            //     table: "settlement");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.AddColumn<int>(
            //     name: "StatusPaymentEnum",
            //     table: "settlement",
            //     type: "integer",
            //     nullable: false,
            //     defaultValue: 0);
        }
    }
}
