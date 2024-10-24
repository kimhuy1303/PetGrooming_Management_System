using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetGrooming_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class fixEmployeeShiftAndScheduleTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShifts_EmployeeId_ShiftId_Date",
                table: "EmployeeShifts",
                columns: new[] { "EmployeeId", "ShiftId", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShifts_ShiftId",
                table: "EmployeeShifts",
                column: "ShiftId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropIndex(
                name: "IX_EmployeeShifts_EmployeeId_ShiftId_Date",
                table: "EmployeeShifts");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeShifts_ShiftId",
                table: "EmployeeShifts");

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
    }
}
