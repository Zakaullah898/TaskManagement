using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeToAssignUserRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignUserRoles_AppUsers_UserId",
                table: "AssignUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignUserRoles_UserRoles_RoleId",
                table: "AssignUserRoles");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignUserRoles_Roles_RoleId",
                table: "AssignUserRoles",
                column: "RoleId",
                principalTable: "UserRoles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignUserRoles_Users_UserId",
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
                name: "FK_AssignUserRoles_Roles_RoleId",
                table: "AssignUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignUserRoles_Users_UserId",
                table: "AssignUserRoles");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignUserRoles_AppUsers_UserId",
                table: "AssignUserRoles",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignUserRoles_UserRoles_RoleId",
                table: "AssignUserRoles",
                column: "RoleId",
                principalTable: "UserRoles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
