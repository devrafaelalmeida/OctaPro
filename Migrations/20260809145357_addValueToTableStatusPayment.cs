using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OctaPro.Migrations
{
    /// <inheritdoc />
    public partial class addValueToTableStatusPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO status_payment (description)
                SELECT 'Estornada'
                WHERE NOT EXISTS (
                    SELECT 1 FROM status_payment WHERE description = 'Estornada'
                );
            ");        
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM status_payment WHERE description = 'Estornada';");
        }
    }
}
