using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QDR_Server.Migrations
{
    /// <inheritdoc />
    public partial class Fixedthedefaultrolefromusertomember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "Member",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "User",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "Member");
        }
    }
}
