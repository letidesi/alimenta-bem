using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlimentaBem.Migrations
{
    /// <inheritdoc />
    public partial class DonationStatusFlow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "receivedAt",
                table: "Donations",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "reviewedAt",
                table: "Donations",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Donations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "unavailableMessage",
                table: "Donations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "unavailableReason",
                table: "Donations",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "receivedAt",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "reviewedAt",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "unavailableMessage",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "unavailableReason",
                table: "Donations");
        }
    }
}
