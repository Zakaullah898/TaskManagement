using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeToAssignTaskTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_AppUsers_AssignedToUserId",
                table: "TaskAssignments");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignment_AppUser",
                table: "TaskAssignments",
                column: "AssignedToUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignment_AppUser",
                table: "TaskAssignments");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignments_AppUsers_AssignedToUserId",
                table: "TaskAssignments",
                column: "AssignedToUserId",
                principalTable: "AppUsers",
                principalColumn: "Id");
        }
    }
}
