using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QDR_Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedexplicitUserOrganizationsmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserOrganizations_Organizations_OrganizationsId",
                table: "UserOrganizations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserOrganizations_Users_UsersId",
                table: "UserOrganizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserOrganizations",
                table: "UserOrganizations");

            migrationBuilder.RenameTable(
                name: "UserOrganizations",
                newName: "OrganizationUser");

            migrationBuilder.RenameIndex(
                name: "IX_UserOrganizations_UsersId",
                table: "OrganizationUser",
                newName: "IX_OrganizationUser_UsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationUser",
                table: "OrganizationUser",
                columns: new[] { "OrganizationsId", "UsersId" });

            migrationBuilder.CreateTable(
                name: "UserOrganization",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOrganization", x => new { x.UserId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_UserOrganization_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOrganization_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserOrganization_OrganizationId",
                table: "UserOrganization",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationUser_Organizations_OrganizationsId",
                table: "OrganizationUser",
                column: "OrganizationsId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationUser_Users_UsersId",
                table: "OrganizationUser",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationUser_Organizations_OrganizationsId",
                table: "OrganizationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationUser_Users_UsersId",
                table: "OrganizationUser");

            migrationBuilder.DropTable(
                name: "UserOrganization");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationUser",
                table: "OrganizationUser");

            migrationBuilder.RenameTable(
                name: "OrganizationUser",
                newName: "UserOrganizations");

            migrationBuilder.RenameIndex(
                name: "IX_OrganizationUser_UsersId",
                table: "UserOrganizations",
                newName: "IX_UserOrganizations_UsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserOrganizations",
                table: "UserOrganizations",
                columns: new[] { "OrganizationsId", "UsersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrganizations_Organizations_OrganizationsId",
                table: "UserOrganizations",
                column: "OrganizationsId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrganizations_Users_UsersId",
                table: "UserOrganizations",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
