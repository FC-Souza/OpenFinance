using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.OpenFinance.Migrations
{
    /// <inheritdoc />
    public partial class AddFailedLoginFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "blocked_until",
                table: "client_credential",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_blocked",
                table: "client_credential",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "blocked_until",
                table: "client_credential");

            migrationBuilder.DropColumn(
                name: "is_blocked",
                table: "client_credential");
        }
    }
}
