using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OctaPro.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableCorporations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "corporations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    id_public = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    legal_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    trade_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    cnpj = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    opening_date = table.Column<DateOnly>(type: "date", nullable: false),
                    state_registration = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    municipal_registration = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    tax_regime = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    zip_code = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    street = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    complement = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    district = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    city = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    state = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    mobile = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("corporations_pkey", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "corporations_id_public_key",
                table: "corporations",
                column: "id_public",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "corporations");
        }
    }
}
