using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetGrooming_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class fixEmployeeShiftAndScheduleTable3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeShifts",
                table: "EmployeeShifts",
                columns: new[] { "EmployeeId", "ShiftId", "Date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeShifts",
                table: "EmployeeShifts");

            
        }
    }
}
