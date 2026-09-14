using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QDR_Server.Migrations
{
    /// <inheritdoc />
    public partial class Somethignnewaddedidk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserOrganization_Organizations_OrganizationId",
                table: "UserOrganization");

            migrationBuilder.DropForeignKey(
                name: "FK_UserOrganization_Users_UserId",
                table: "UserOrganization");

            migrationBuilder.DropTable(
                name: "OrganizationUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserOrganization",
                table: "UserOrganization");

            migrationBuilder.RenameTable(
                name: "UserOrganization",
                newName: "UserOrganizations");

            migrationBuilder.RenameIndex(
                name: "IX_UserOrganization_OrganizationId",
                table: "UserOrganizations",
                newName: "IX_UserOrganizations_OrganizationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserOrganizations",
                table: "UserOrganizations",
                columns: new[] { "UserId", "OrganizationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrganizations_Organizations_OrganizationId",
                table: "UserOrganizations",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrganizations_Users_UserId",
                table: "UserOrganizations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserOrganizations_Organizations_OrganizationId",
                table: "UserOrganizations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserOrganizations_Users_UserId",
                table: "UserOrganizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserOrganizations",
                table: "UserOrganizations");

            migrationBuilder.RenameTable(
                name: "UserOrganizations",
                newName: "UserOrganization");

            migrationBuilder.RenameIndex(
                name: "IX_UserOrganizations_OrganizationId",
                table: "UserOrganization",
                newName: "IX_UserOrganization_OrganizationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserOrganization",
                table: "UserOrganization",
                columns: new[] { "UserId", "OrganizationId" });

            migrationBuilder.CreateTable(
                name: "OrganizationUser",
                columns: table => new
                {
                    OrganizationsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationUser", x => new { x.OrganizationsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_OrganizationUser_Organizations_OrganizationsId",
                        column: x => x.OrganizationsId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUser_UsersId",
                table: "OrganizationUser",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrganization_Organizations_OrganizationId",
                table: "UserOrganization",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrganization_Users_UserId",
                table: "UserOrganization",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
