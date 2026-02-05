using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingTaskAssignmenTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskTable_AppUsers_CreatedByUserId",
                table: "TaskTable");

            migrationBuilder.DropIndex(
                name: "IX_TaskTable_CreatedByUserId",
                table: "TaskTable");



            migrationBuilder.CreateTable(
                name: "TaskAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    AssignedToUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AssignedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskAssignments_AppUsers_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskAssignments_TaskTable_TaskId",
                        column: x => x.TaskId,
                        principalTable: "TaskTable",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Cascade);
                });



            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignments_AssignedToUserId",
                table: "TaskAssignments",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignments_TaskId",
                table: "TaskAssignments",
                column: "TaskId");

          
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskTable_AppUsers_UserId",
                table: "TaskTable");

            migrationBuilder.DropTable(
                name: "TaskAssignments");

            migrationBuilder.DropIndex(
                name: "IX_TaskTable_UserId",
                table: "TaskTable");


            migrationBuilder.CreateIndex(
                name: "IX_TaskTable_CreatedByUserId",
                table: "TaskTable",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTable_AppUsers_CreatedByUserId",
                table: "TaskTable",
                column: "CreatedByUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
