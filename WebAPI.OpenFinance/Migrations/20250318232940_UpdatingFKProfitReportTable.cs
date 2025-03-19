using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.OpenFinance.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingFKProfitReportTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_profit_report_product_types_product_types",
                table: "profit_report");

            migrationBuilder.DropIndex(
                name: "IX_profit_report_product_types",
                table: "profit_report");

            migrationBuilder.DropColumn(
                name: "product_types",
                table: "profit_report");

            migrationBuilder.CreateIndex(
                name: "IX_profit_report_product_id",
                table: "profit_report",
                column: "product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_profit_report_product_types_product_id",
                table: "profit_report",
                column: "product_id",
                principalTable: "product_types",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_profit_report_product_types_product_id",
                table: "profit_report");

            migrationBuilder.DropIndex(
                name: "IX_profit_report_product_id",
                table: "profit_report");

            migrationBuilder.AddColumn<int>(
                name: "product_types",
                table: "profit_report",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_profit_report_product_types",
                table: "profit_report",
                column: "product_types");

            migrationBuilder.AddForeignKey(
                name: "FK_profit_report_product_types_product_types",
                table: "profit_report",
                column: "product_types",
                principalTable: "product_types",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
