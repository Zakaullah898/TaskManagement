using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addingPropertyToOtpTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Otp",
                table: "OTPs",
                newName: "OtpHash");

            migrationBuilder.AddColumn<int>(
                name: "AttemptCount",
                table: "OTPs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "OTPs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsUsed",
                table: "OTPs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttemptCount",
                table: "OTPs");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "OTPs");

            migrationBuilder.DropColumn(
                name: "IsUsed",
                table: "OTPs");

            migrationBuilder.RenameColumn(
                name: "OtpHash",
                table: "OTPs",
                newName: "Otp");
        }
    }
}
