using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebAPI.OpenFinance.Migrations
{
    /// <inheritdoc />
    public partial class AddingBenchmarkIndexTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "benchmark_index",
                columns: table => new
                {
                    benchmark_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    benchmark_period = table.Column<DateTime>(type: "date", nullable: false),
                    sp_tsx = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    cpi = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    cbpr = table.Column<decimal>(type: "numeric(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_benchmark_index", x => x.benchmark_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "benchmark_index");
        }
    }
}
