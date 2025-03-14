using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebAPI.OpenFinance.Migrations
{
    /// <inheritdoc />
    public partial class AddingStatementTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "statement",
                columns: table => new
                {
                    statement_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    connection_id = table.Column<int>(type: "integer", nullable: false),
                    connectionId = table.Column<int>(type: "integer", nullable: true),
                    statement_month = table.Column<int>(type: "integer", nullable: false),
                    statement_year = table.Column<int>(type: "integer", nullable: false),
                    last_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statement", x => x.statement_id);
                    table.ForeignKey(
                        name: "FK_statement_connections_connectionId",
                        column: x => x.connectionId,
                        principalTable: "connections",
                        principalColumn: "connection_id");
                });

            migrationBuilder.CreateTable(
                name: "transaction_direction",
                columns: table => new
                {
                    transaction_direction_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    transaction_direction_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_direction", x => x.transaction_direction_id);
                });

            migrationBuilder.CreateTable(
                name: "transaction_type",
                columns: table => new
                {
                    transaction_type_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    transaction_type_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_type", x => x.transaction_type_id);
                });

            migrationBuilder.CreateTable(
                name: "transaction",
                columns: table => new
                {
                    transaction_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    statement_id = table.Column<int>(type: "integer", nullable: false),
                    transaction_type_id = table.Column<int>(type: "integer", nullable: false),
                    transaction_direction_id = table.Column<int>(type: "integer", nullable: false),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    asset_name = table.Column<string>(type: "text", nullable: false),
                    transaction_date = table.Column<DateTime>(type: "date", nullable: false),
                    transaction_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction", x => x.transaction_id);
                    table.ForeignKey(
                        name: "FK_transaction_statement_statement_id",
                        column: x => x.statement_id,
                        principalTable: "statement",
                        principalColumn: "statement_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transaction_transaction_direction_transaction_direction_id",
                        column: x => x.transaction_direction_id,
                        principalTable: "transaction_direction",
                        principalColumn: "transaction_direction_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transaction_transaction_type_transaction_type_id",
                        column: x => x.transaction_type_id,
                        principalTable: "transaction_type",
                        principalColumn: "transaction_type_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_statement_connectionId",
                table: "statement",
                column: "connectionId");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_statement_id",
                table: "transaction",
                column: "statement_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_transaction_direction_id",
                table: "transaction",
                column: "transaction_direction_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_transaction_type_id",
                table: "transaction",
                column: "transaction_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transaction");

            migrationBuilder.DropTable(
                name: "statement");

            migrationBuilder.DropTable(
                name: "transaction_direction");

            migrationBuilder.DropTable(
                name: "transaction_type");

            migrationBuilder.DropColumn(
                name: "status",
                table: "connections");
        }
    }
}
