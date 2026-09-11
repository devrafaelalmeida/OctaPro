using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OctaPro.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnFullNameToTableUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "users");
        }
    }
}
