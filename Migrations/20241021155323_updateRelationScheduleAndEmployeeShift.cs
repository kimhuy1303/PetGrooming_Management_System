using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetGrooming_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class updateRelationScheduleAndEmployeeShift : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_EmployeeShifts_EmployeeShiftEmployeeId_EmployeeShiftShiftId",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Shift_ShiftId",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Users_EmployeeId",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_EmployeeId",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_EmployeeShiftEmployeeId_EmployeeShiftShiftId",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_ShiftId",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "EmployeeShiftEmployeeId",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "EmployeeShiftShiftId",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "ShiftId",
                table: "Schedule");

            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "EmployeeShifts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShifts_ScheduleId",
                table: "EmployeeShifts",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeShifts_Schedule_ScheduleId",
                table: "EmployeeShifts",
                column: "ScheduleId",
                principalTable: "Schedule",
                principalColumn: "Id_Schedule");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeShifts_Schedule_ScheduleId",
                table: "EmployeeShifts");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeShifts_ScheduleId",
                table: "EmployeeShifts");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "EmployeeShifts");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Schedule",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Schedule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeShiftEmployeeId",
                table: "Schedule",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeShiftShiftId",
                table: "Schedule",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShiftId",
                table: "Schedule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_EmployeeId",
                table: "Schedule",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_EmployeeShiftEmployeeId_EmployeeShiftShiftId",
                table: "Schedule",
                columns: new[] { "EmployeeShiftEmployeeId", "EmployeeShiftShiftId" });

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_ShiftId",
                table: "Schedule",
                column: "ShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_EmployeeShifts_EmployeeShiftEmployeeId_EmployeeShiftShiftId",
                table: "Schedule",
                columns: new[] { "EmployeeShiftEmployeeId", "EmployeeShiftShiftId" },
                principalTable: "EmployeeShifts",
                principalColumns: new[] { "EmployeeId", "ShiftId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Shift_ShiftId",
                table: "Schedule",
                column: "ShiftId",
                principalTable: "Shift",
                principalColumn: "Id_Shift",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Users_EmployeeId",
                table: "Schedule",
                column: "EmployeeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
