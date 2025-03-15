using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebAPI.OpenFinance.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStatementTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transaction_statement_statement_id",
                table: "transaction");

            migrationBuilder.DropTable(
                name: "statement");

            migrationBuilder.DropIndex(
                name: "IX_transaction_statement_id",
                table: "transaction");

            migrationBuilder.RenameColumn(
                name: "statement_id",
                table: "transaction",
                newName: "connection_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "connection_id",
                table: "transaction",
                newName: "statement_id");

            migrationBuilder.CreateTable(
                name: "statement",
                columns: table => new
                {
                    statement_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    connection_id = table.Column<int>(type: "integer", nullable: false),
                    last_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    statement_month = table.Column<int>(type: "integer", nullable: false),
                    statement_year = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statement", x => x.statement_id);
                    table.ForeignKey(
                        name: "FK_statement_connections_connection_id",
                        column: x => x.connection_id,
                        principalTable: "connections",
                        principalColumn: "connection_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_transaction_statement_id",
                table: "transaction",
                column: "statement_id");

            migrationBuilder.CreateIndex(
                name: "IX_statement_connection_id",
                table: "statement",
                column: "connection_id");

            migrationBuilder.AddForeignKey(
                name: "FK_transaction_statement_statement_id",
                table: "transaction",
                column: "statement_id",
                principalTable: "statement",
                principalColumn: "statement_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
