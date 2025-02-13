using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebAPI.OpenFinance.Migrations
{
    /// <inheritdoc />
    public partial class AddMutualFundTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mutual_fund",
                columns: table => new
                {
                    mf_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    mf_name = table.Column<string>(type: "text", nullable: false),
                    mf_symbol = table.Column<string>(type: "text", nullable: false),
                    mf_type = table.Column<string>(type: "text", nullable: false),
                    mf_currency = table.Column<string>(type: "text", nullable: false),
                    mf_last_nav = table.Column<decimal>(type: "numeric", nullable: false),
                    mf_inception_date = table.Column<DateOnly>(type: "date", nullable: false),
                    mf_management_fee = table.Column<decimal>(type: "numeric", nullable: false),
                    last_updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mutual_fund", x => x.mf_id);
                });

            migrationBuilder.CreateTable(
                name: "mutual_fund_info",
                columns: table => new
                {
                    mfi_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mf_id = table.Column<int>(type: "integer", nullable: false),
                    connection_id = table.Column<int>(type: "integer", nullable: false),
                    connectionID = table.Column<int>(type: "integer", nullable: false),
                    quantity_shares = table.Column<int>(type: "integer", nullable: false),
                    average_nav = table.Column<decimal>(type: "numeric", nullable: false),
                    last_updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mutual_fund_info", x => x.mfi_id);
                    table.ForeignKey(
                        name: "FK_mutual_fund_info_connections_connectionID",
                        column: x => x.connectionID,
                        principalTable: "connections",
                        principalColumn: "connection_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mutual_fund_info_mutual_fund_mf_id",
                        column: x => x.mf_id,
                        principalTable: "mutual_fund",
                        principalColumn: "mf_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_mutual_fund_info_connectionID",
                table: "mutual_fund_info",
                column: "connectionID");

            migrationBuilder.CreateIndex(
                name: "IX_mutual_fund_info_mf_id",
                table: "mutual_fund_info",
                column: "mf_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mutual_fund_info");

            migrationBuilder.DropTable(
                name: "mutual_fund");
        }
    }
}
