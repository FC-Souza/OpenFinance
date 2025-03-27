using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.OpenFinance.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingFKproductId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mutual_fund_product_types_product_types",
                table: "mutual_fund");

            migrationBuilder.DropForeignKey(
                name: "FK_stock_product_types_product_types",
                table: "stock");

            migrationBuilder.DropIndex(
                name: "IX_stock_product_types",
                table: "stock");

            migrationBuilder.DropIndex(
                name: "IX_mutual_fund_product_types",
                table: "mutual_fund");

            migrationBuilder.DropColumn(
                name: "product_types",
                table: "stock");

            migrationBuilder.DropColumn(
                name: "product_types",
                table: "mutual_fund");

            migrationBuilder.CreateIndex(
                name: "IX_stock_product_id",
                table: "stock",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_mutual_fund_product_id",
                table: "mutual_fund",
                column: "product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_mutual_fund_product_types_product_id",
                table: "mutual_fund",
                column: "product_id",
                principalTable: "product_types",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_stock_product_types_product_id",
                table: "stock",
                column: "product_id",
                principalTable: "product_types",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mutual_fund_product_types_product_id",
                table: "mutual_fund");

            migrationBuilder.DropForeignKey(
                name: "FK_stock_product_types_product_id",
                table: "stock");

            migrationBuilder.DropIndex(
                name: "IX_stock_product_id",
                table: "stock");

            migrationBuilder.DropIndex(
                name: "IX_mutual_fund_product_id",
                table: "mutual_fund");

            migrationBuilder.AddColumn<int>(
                name: "product_types",
                table: "stock",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "product_types",
                table: "mutual_fund",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_stock_product_types",
                table: "stock",
                column: "product_types");

            migrationBuilder.CreateIndex(
                name: "IX_mutual_fund_product_types",
                table: "mutual_fund",
                column: "product_types");

            migrationBuilder.AddForeignKey(
                name: "FK_mutual_fund_product_types_product_types",
                table: "mutual_fund",
                column: "product_types",
                principalTable: "product_types",
                principalColumn: "product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_stock_product_types_product_types",
                table: "stock",
                column: "product_types",
                principalTable: "product_types",
                principalColumn: "product_id");
        }
    }
}
