using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.OpenFinance.Migrations
{
    /// <inheritdoc />
    public partial class AddMutualFundProdFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "product_types",
                table: "mutual_fund",
                type: "integer",
                nullable: true);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mutual_fund_product_types_product_types",
                table: "mutual_fund");

            migrationBuilder.DropIndex(
                name: "IX_mutual_fund_product_types",
                table: "mutual_fund");

            migrationBuilder.DropColumn(
                name: "product_types",
                table: "mutual_fund");
        }
    }
}
