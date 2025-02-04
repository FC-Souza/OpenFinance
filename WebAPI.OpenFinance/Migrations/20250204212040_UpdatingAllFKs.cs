using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.OpenFinance.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingAllFKs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "product_types",
                table: "stock",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "product_types",
                table: "cash",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_stock_info_connection_id",
                table: "stock_info",
                column: "connection_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_info_stock_id",
                table: "stock_info",
                column: "stock_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_product_types",
                table: "stock",
                column: "product_types");

            migrationBuilder.CreateIndex(
                name: "IX_connections_bank_id",
                table: "connections",
                column: "bank_id");

            migrationBuilder.CreateIndex(
                name: "IX_cash_info_cash_id",
                table: "cash_info",
                column: "cash_id");

            migrationBuilder.CreateIndex(
                name: "IX_cash_info_connection_id",
                table: "cash_info",
                column: "connection_id");

            migrationBuilder.CreateIndex(
                name: "IX_cash_product_types",
                table: "cash",
                column: "product_types");

            migrationBuilder.AddForeignKey(
                name: "FK_cash_product_types_product_types",
                table: "cash",
                column: "product_types",
                principalTable: "product_types",
                principalColumn: "product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_cash_info_cash_cash_id",
                table: "cash_info",
                column: "cash_id",
                principalTable: "cash",
                principalColumn: "cash_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cash_info_connections_connection_id",
                table: "cash_info",
                column: "connection_id",
                principalTable: "connections",
                principalColumn: "connection_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_connections_banks_bank_id",
                table: "connections",
                column: "bank_id",
                principalTable: "banks",
                principalColumn: "bank_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_stock_product_types_product_types",
                table: "stock",
                column: "product_types",
                principalTable: "product_types",
                principalColumn: "product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_stock_info_connections_connection_id",
                table: "stock_info",
                column: "connection_id",
                principalTable: "connections",
                principalColumn: "connection_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_stock_info_stock_stock_id",
                table: "stock_info",
                column: "stock_id",
                principalTable: "stock",
                principalColumn: "stock_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cash_product_types_product_types",
                table: "cash");

            migrationBuilder.DropForeignKey(
                name: "FK_cash_info_cash_cash_id",
                table: "cash_info");

            migrationBuilder.DropForeignKey(
                name: "FK_cash_info_connections_connection_id",
                table: "cash_info");

            migrationBuilder.DropForeignKey(
                name: "FK_connections_banks_bank_id",
                table: "connections");

            migrationBuilder.DropForeignKey(
                name: "FK_stock_product_types_product_types",
                table: "stock");

            migrationBuilder.DropForeignKey(
                name: "FK_stock_info_connections_connection_id",
                table: "stock_info");

            migrationBuilder.DropForeignKey(
                name: "FK_stock_info_stock_stock_id",
                table: "stock_info");

            migrationBuilder.DropIndex(
                name: "IX_stock_info_connection_id",
                table: "stock_info");

            migrationBuilder.DropIndex(
                name: "IX_stock_info_stock_id",
                table: "stock_info");

            migrationBuilder.DropIndex(
                name: "IX_stock_product_types",
                table: "stock");

            migrationBuilder.DropIndex(
                name: "IX_connections_bank_id",
                table: "connections");

            migrationBuilder.DropIndex(
                name: "IX_cash_info_cash_id",
                table: "cash_info");

            migrationBuilder.DropIndex(
                name: "IX_cash_info_connection_id",
                table: "cash_info");

            migrationBuilder.DropIndex(
                name: "IX_cash_product_types",
                table: "cash");

            migrationBuilder.DropColumn(
                name: "product_types",
                table: "stock");

            migrationBuilder.DropColumn(
                name: "product_types",
                table: "cash");
        }
    }
}
