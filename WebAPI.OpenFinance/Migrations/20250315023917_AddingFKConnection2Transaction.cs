using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.OpenFinance.Migrations
{
    /// <inheritdoc />
    public partial class AddingFKConnection2Transaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_transaction_connection_id",
                table: "transaction",
                column: "connection_id");

            migrationBuilder.AddForeignKey(
                name: "FK_transaction_connections_connection_id",
                table: "transaction",
                column: "connection_id",
                principalTable: "connections",
                principalColumn: "connection_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transaction_connections_connection_id",
                table: "transaction");

            migrationBuilder.DropIndex(
                name: "IX_transaction_connection_id",
                table: "transaction");
        }
    }
}
