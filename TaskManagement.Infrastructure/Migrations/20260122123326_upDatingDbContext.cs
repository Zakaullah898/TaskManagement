using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class upDatingDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignUserRole_AppUsers_UserId",
                table: "AssignUserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignUserRole_UserRoles_RoleId",
                table: "AssignUserRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssignUserRole",
                table: "AssignUserRole");

            migrationBuilder.RenameTable(
                name: "AssignUserRole",
                newName: "AssignUserRoles");

            migrationBuilder.RenameIndex(
                name: "IX_AssignUserRole_UserId",
                table: "AssignUserRoles",
                newName: "IX_AssignUserRoles_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AssignUserRole_RoleId",
                table: "AssignUserRoles",
                newName: "IX_AssignUserRoles_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssignUserRoles",
                table: "AssignUserRoles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignUserRoles_AppUsers_UserId",
                table: "AssignUserRoles",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignUserRoles_UserRoles_RoleId",
                table: "AssignUserRoles",
                column: "RoleId",
                principalTable: "UserRoles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignUserRoles_AppUsers_UserId",
                table: "AssignUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignUserRoles_UserRoles_RoleId",
                table: "AssignUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssignUserRoles",
                table: "AssignUserRoles");

            migrationBuilder.RenameTable(
                name: "AssignUserRoles",
                newName: "AssignUserRole");

            migrationBuilder.RenameIndex(
                name: "IX_AssignUserRoles_UserId",
                table: "AssignUserRole",
                newName: "IX_AssignUserRole_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AssignUserRoles_RoleId",
                table: "AssignUserRole",
                newName: "IX_AssignUserRole_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssignUserRole",
                table: "AssignUserRole",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignUserRole_AppUsers_UserId",
                table: "AssignUserRole",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignUserRole_UserRoles_RoleId",
                table: "AssignUserRole",
                column: "RoleId",
                principalTable: "UserRoles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
