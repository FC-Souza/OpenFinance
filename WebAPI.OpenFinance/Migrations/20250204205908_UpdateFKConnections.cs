using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.OpenFinance.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFKConnections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_connections_client_id",
                table: "connections",
                column: "client_id");

            migrationBuilder.AddForeignKey(
                name: "FK_connections_clients_client_id",
                table: "connections",
                column: "client_id",
                principalTable: "clients",
                principalColumn: "client_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_connections_clients_client_id",
                table: "connections");

            migrationBuilder.DropIndex(
                name: "IX_connections_client_id",
                table: "connections");
        }
    }
}
