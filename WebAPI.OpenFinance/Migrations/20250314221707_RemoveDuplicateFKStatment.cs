using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.OpenFinance.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDuplicateFKStatment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_statement_connections_connectionId",
                table: "statement");

            migrationBuilder.DropIndex(
                name: "IX_statement_connectionId",
                table: "statement");

            migrationBuilder.DropColumn(
                name: "connectionId",
                table: "statement");

            migrationBuilder.CreateIndex(
                name: "IX_statement_connection_id",
                table: "statement",
                column: "connection_id");

            migrationBuilder.AddForeignKey(
                name: "FK_statement_connections_connection_id",
                table: "statement",
                column: "connection_id",
                principalTable: "connections",
                principalColumn: "connection_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_statement_connections_connection_id",
                table: "statement");

            migrationBuilder.DropIndex(
                name: "IX_statement_connection_id",
                table: "statement");

            migrationBuilder.AddColumn<int>(
                name: "connectionId",
                table: "statement",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_statement_connectionId",
                table: "statement",
                column: "connectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_statement_connections_connectionId",
                table: "statement",
                column: "connectionId",
                principalTable: "connections",
                principalColumn: "connection_id");
        }
    }
}
