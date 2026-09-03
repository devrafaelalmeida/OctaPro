using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OctaPro.Migrations.TenantDbContext
{
    /// <inheritdoc />
    public partial class InitTenants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tenants",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    domain = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    connection_name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    data_source = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    database = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    username = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    ativo = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    criado_em = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tenants", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tenants_domain",
                table: "tenants",
                column: "domain",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tenants");
        }
    }
}
