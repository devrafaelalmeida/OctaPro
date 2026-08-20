using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OctaPro.Migrations
{
    /// <inheritdoc />
    public partial class UnifyInstallmentsTph : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_settlement_installments_settlement_settlement_id",
                table: "settlement_installments");

            migrationBuilder.DropTable(
                name: "legal_fees_installments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SettlementInstallments",
                table: "settlement_installments");

            migrationBuilder.RenameTable(
                name: "settlement_installments",
                newName: "installments");

            migrationBuilder.RenameColumn(
                name: "settlement_id",
                table: "installments",
                newName: "referency_id");

            migrationBuilder.RenameIndex(
                name: "IX_settlement_installments_settlement_id",
                table: "installments",
                newName: "IX_installments_referency_id");

            migrationBuilder.AlterColumn<decimal>(
                name: "ValueInstallment",
                table: "installments",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "installments",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "type_id",
                table: "installments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_installments",
                table: "installments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "type_installments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_type_installments", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "type_installments",
                columns: new[] { "id", "description" },
                values: new object[,]
                {
                    { 1, "Acordo" },
                    { 2, "Honorário" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_installments_type_id",
                table: "installments",
                column: "type_id");

            migrationBuilder.AddForeignKey(
                name: "FK_installments_settlement_referency_id",
                table: "installments",
                column: "referency_id",
                principalTable: "settlement",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            // migrationBuilder.AddForeignKey(
            //     name: "FK_installments_type_installments_type_id",
            //     table: "installments",
            //     column: "type_id",
            //     principalTable: "type_installments",
            //     principalColumn: "id",
            //     onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_installments_settlement_referency_id",
                table: "installments");

            migrationBuilder.DropForeignKey(
                name: "FK_installments_type_installments_type_id",
                table: "installments");

            migrationBuilder.DropTable(
                name: "type_installments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_installments",
                table: "installments");

            migrationBuilder.DropIndex(
                name: "IX_installments_type_id",
                table: "installments");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "installments");

            migrationBuilder.DropColumn(
                name: "type_id",
                table: "installments");

            migrationBuilder.RenameTable(
                name: "installments",
                newName: "settlement_installments");

            migrationBuilder.RenameColumn(
                name: "referency_id",
                table: "settlement_installments",
                newName: "settlement_id");

            migrationBuilder.RenameIndex(
                name: "IX_installments_referency_id",
                table: "settlement_installments",
                newName: "IX_settlement_installments_settlement_id");

            migrationBuilder.AlterColumn<decimal>(
                name: "ValueInstallment",
                table: "settlement_installments",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)",
                oldPrecision: 10,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_settlement_installments",
                table: "settlement_installments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "legal_fees_installments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    entity_id = table.Column<long>(type: "bigint", nullable: false),
                    status_payment_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    current_installment = table.Column<int>(type: "integer", nullable: false),
                    due_date = table.Column<DateOnly>(type: "date", nullable: true),
                    id_public = table.Column<Guid>(type: "uuid", nullable: false),
                    legal_fee_id = table.Column<int>(type: "integer", nullable: false),
                    note = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    payment_date = table.Column<DateOnly>(type: "date", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    value_installment = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("installments_legal_fees_pkey", x => x.id);
                    table.ForeignKey(
                        name: "FK_legal_fees_installments_status_payment_status_payment_id",
                        column: x => x.status_payment_id,
                        principalTable: "status_payment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_legal_fee_entity",
                        column: x => x.entity_id,
                        principalTable: "entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "installments_legal_fees_id_public_key",
                table: "legal_fees_installments",
                column: "id_public",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_legal_fees_installments_entity_id",
                table: "legal_fees_installments",
                column: "entity_id");

            migrationBuilder.CreateIndex(
                name: "IX_legal_fees_installments_status_payment_id",
                table: "legal_fees_installments",
                column: "status_payment_id");

            migrationBuilder.AddForeignKey(
                name: "FK_settlement_installments_settlement_settlement_id",
                table: "settlement_installments",
                column: "settlement_id",
                principalTable: "settlement",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
