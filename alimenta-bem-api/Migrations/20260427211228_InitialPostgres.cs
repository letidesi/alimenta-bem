using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlimentaBem.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    createdAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    passwordHash = table.Column<string>(type: "text", nullable: false),
                    createdAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationRequirements",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    organizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    itemName = table.Column<string>(type: "text", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    createdAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationRequirements", x => x.id);
                    table.ForeignKey(
                        name: "FK_OrganizationRequirements_Organizations_organizationId",
                        column: x => x.organizationId,
                        principalTable: "Organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NaturalPersons",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    userId = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    emailUser = table.Column<string>(type: "text", nullable: false),
                    socialName = table.Column<string>(type: "text", nullable: true),
                    age = table.Column<string>(type: "text", nullable: false),
                    birthdayDate = table.Column<DateOnly>(type: "date", nullable: false),
                    gender = table.Column<int>(type: "integer", nullable: true),
                    skinColor = table.Column<int>(type: "integer", nullable: true),
                    isPcd = table.Column<bool>(type: "boolean", nullable: true),
                    createdAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaturalPersons", x => x.id);
                    table.ForeignKey(
                        name: "FK_NaturalPersons_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    userId = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    createdAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.id);
                    table.ForeignKey(
                        name: "FK_Roles_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Donations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    naturalPersonId = table.Column<Guid>(type: "uuid", nullable: false),
                    organizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    itemName = table.Column<string>(type: "text", nullable: false),
                    amountDonated = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    unavailableReason = table.Column<string>(type: "text", nullable: true),
                    unavailableMessage = table.Column<string>(type: "text", nullable: true),
                    reviewedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    receivedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    createdAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donations", x => x.id);
                    table.ForeignKey(
                        name: "FK_Donations_NaturalPersons_naturalPersonId",
                        column: x => x.naturalPersonId,
                        principalTable: "NaturalPersons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Donations_Organizations_organizationId",
                        column: x => x.organizationId,
                        principalTable: "Organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Donations_naturalPersonId",
                table: "Donations",
                column: "naturalPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_organizationId",
                table: "Donations",
                column: "organizationId");

            migrationBuilder.CreateIndex(
                name: "IX_NaturalPersons_userId",
                table: "NaturalPersons",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationRequirements_organizationId",
                table: "OrganizationRequirements",
                column: "organizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_userId",
                table: "Roles",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Donations");

            migrationBuilder.DropTable(
                name: "OrganizationRequirements");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "NaturalPersons");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
