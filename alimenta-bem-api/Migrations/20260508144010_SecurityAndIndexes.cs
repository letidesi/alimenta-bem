using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlimentaBem.Migrations
{
    /// <inheritdoc />
    public partial class SecurityAndIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationRequirements_Organizations_organizationId",
                table: "OrganizationRequirements");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationRequirements_organizationId",
                table: "OrganizationRequirements");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Users",
                type: "varchar(200)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "Users",
                type: "varchar(200)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "Roles",
                type: "varchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "token",
                table: "PasswordResetTokens",
                type: "varchar(64)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "Organizations",
                type: "varchar(40)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Organizations",
                type: "varchar(150)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "Organizations",
                type: "varchar(500)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "OrganizationRequirements",
                type: "varchar(40)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "itemName",
                table: "OrganizationRequirements",
                type: "varchar(150)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "socialName",
                table: "NaturalPersons",
                type: "varchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "skinColor",
                table: "NaturalPersons",
                type: "varchar(100)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "NaturalPersons",
                type: "varchar(200)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "isPcd",
                table: "NaturalPersons",
                type: "varchar(100)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "gender",
                table: "NaturalPersons",
                type: "varchar(100)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "emailUser",
                table: "NaturalPersons",
                type: "varchar(200)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "birthdayDate",
                table: "NaturalPersons",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "age",
                table: "NaturalPersons",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "unavailableReason",
                table: "Donations",
                type: "varchar(80)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "unavailableMessage",
                table: "Donations",
                type: "varchar(300)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "Donations",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "itemName",
                table: "Donations",
                type: "varchar(150)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_Users_email",
                table: "Users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetTokens_token",
                table: "PasswordResetTokens",
                column: "token");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetTokens_userId_used",
                table: "PasswordResetTokens",
                columns: new[] { "userId", "used" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationRequirements_Organizations_id",
                table: "OrganizationRequirements",
                column: "id",
                principalTable: "Organizations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationRequirements_Organizations_id",
                table: "OrganizationRequirements");

            migrationBuilder.DropIndex(
                name: "IX_Users_email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_PasswordResetTokens_token",
                table: "PasswordResetTokens");

            migrationBuilder.DropIndex(
                name: "IX_PasswordResetTokens_userId_used",
                table: "PasswordResetTokens");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)");

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "Roles",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)");

            migrationBuilder.AlterColumn<string>(
                name: "token",
                table: "PasswordResetTokens",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(64)");

            migrationBuilder.AlterColumn<int>(
                name: "type",
                table: "Organizations",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(40)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Organizations",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(150)");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "Organizations",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "type",
                table: "OrganizationRequirements",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(40)");

            migrationBuilder.AlterColumn<string>(
                name: "itemName",
                table: "OrganizationRequirements",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(150)");

            migrationBuilder.AlterColumn<string>(
                name: "socialName",
                table: "NaturalPersons",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "skinColor",
                table: "NaturalPersons",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "NaturalPersons",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)");

            migrationBuilder.AlterColumn<bool>(
                name: "isPcd",
                table: "NaturalPersons",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "gender",
                table: "NaturalPersons",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "emailUser",
                table: "NaturalPersons",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "birthdayDate",
                table: "NaturalPersons",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<string>(
                name: "age",
                table: "NaturalPersons",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "unavailableReason",
                table: "Donations",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(80)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "unavailableMessage",
                table: "Donations",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(300)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "Donations",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "itemName",
                table: "Donations",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(150)");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationRequirements_organizationId",
                table: "OrganizationRequirements",
                column: "organizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationRequirements_Organizations_organizationId",
                table: "OrganizationRequirements",
                column: "organizationId",
                principalTable: "Organizations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
