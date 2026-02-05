using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeletingUserIdInTaskTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignUserRoles_AppUsers_UserId",
                table: "AssignUserRoles");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AssignUserRoles",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignUserRoles_AppUsers_UserId",
                table: "AssignUserRoles",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignUserRoles_AppUsers_UserId",
                table: "AssignUserRoles");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AssignUserRoles",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignUserRoles_AppUsers_UserId",
                table: "AssignUserRoles",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id");
        }
    }
}
