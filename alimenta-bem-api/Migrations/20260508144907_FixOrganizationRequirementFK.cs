using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlimentaBem.Migrations
{
    /// <inheritdoc />
    public partial class FixOrganizationRequirementFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationRequirements_Organizations_id",
                table: "OrganizationRequirements");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationRequirements_Organizations_organizationId",
                table: "OrganizationRequirements");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationRequirements_organizationId",
                table: "OrganizationRequirements");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationRequirements_Organizations_id",
                table: "OrganizationRequirements",
                column: "id",
                principalTable: "Organizations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
