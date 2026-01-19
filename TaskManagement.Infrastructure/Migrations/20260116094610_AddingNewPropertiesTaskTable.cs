using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingNewPropertiesTaskTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Id",
                table: "TaskTable");

            migrationBuilder.RenameColumn(
                name: "AssignedToID",
                table: "TaskTable",
                newName: "CreatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskTable_AssignedToID",
                table: "TaskTable",
                newName: "IX_TaskTable_CreatedByUserId");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TaskTable",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "TaskTable",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTable_AppUsers_CreatedByUserId",
                table: "TaskTable",
                column: "CreatedByUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskTable_AppUsers_CreatedByUserId",
                table: "TaskTable");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TaskTable");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "TaskTable");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "TaskTable",
                newName: "AssignedToID");

            migrationBuilder.RenameIndex(
                name: "IX_TaskTable_CreatedByUserId",
                table: "TaskTable",
                newName: "IX_TaskTable_AssignedToID");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Id",
                table: "TaskTable",
                column: "AssignedToID",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
