using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetGrooming_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class Add_Relation_EmployeeShiftSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApprove",
                table: "Schedule");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Schedule",
                newName: "Date");

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

            migrationBuilder.CreateTable(
                name: "EmployeeShifts",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ShiftId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeShifts", x => new { x.EmployeeId, x.ShiftId });
                    table.ForeignKey(
                        name: "FK_EmployeeShifts_Shift_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shift",
                        principalColumn: "Id_Shift",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeShifts_Users_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShifts_ShiftId",
                table: "EmployeeShifts",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "EmployeeShifts");

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

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Schedule",
                newName: "DateCreated");

            migrationBuilder.AddColumn<bool>(
                name: "IsApprove",
                table: "Schedule",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
