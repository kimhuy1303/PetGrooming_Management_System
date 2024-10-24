using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetGrooming_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class fixEmployeeShiftAndScheduleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeShifts",
                table: "EmployeeShifts");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeShifts_ShiftId",
                table: "EmployeeShifts");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateEnd",
                table: "Schedule",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateStart",
                table: "Schedule",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeShifts",
                table: "EmployeeShifts",
                columns: new[] { "ShiftId", "EmployeeId" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShifts_EmployeeId",
                table: "EmployeeShifts",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShifts_ShiftId_EmployeeId_Date",
                table: "EmployeeShifts",
                columns: new[] { "ShiftId", "EmployeeId", "Date" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeShifts",
                table: "EmployeeShifts");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeShifts_EmployeeId",
                table: "EmployeeShifts");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeShifts_ShiftId_EmployeeId_Date",
                table: "EmployeeShifts");

            migrationBuilder.DropColumn(
                name: "DateEnd",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "DateStart",
                table: "Schedule");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeShifts",
                table: "EmployeeShifts",
                columns: new[] { "EmployeeId", "ShiftId" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShifts_ShiftId",
                table: "EmployeeShifts",
                column: "ShiftId");
        }
    }
}
