using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetGrooming_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class fixdatabase_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeShifts_Schedule_ScheduleId",
                table: "EmployeeShifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Service_Combo_ComboId",
                table: "Service");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Service_ComboId",
                table: "Service");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeShifts_ScheduleId",
                table: "EmployeeShifts");

            migrationBuilder.DropColumn(
                name: "TotalAppointment",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ComboId",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "PetName",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "WeightPet",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "EmployeeShifts");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Service",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Combo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Combo",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Price",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PriceValue = table.Column<double>(type: "float", nullable: false),
                    PetName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PetWeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Price", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Price_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Price_ServiceId",
                table: "Price",
                column: "ServiceId",
                unique: true,
                filter: "[ServiceId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Price");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Combo");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Combo");

            migrationBuilder.AddColumn<int>(
                name: "TotalAppointment",
                table: "Users",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Service",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<int>(
                name: "ComboId",
                table: "Service",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PetName",
                table: "Service",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Service",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "WeightPet",
                table: "Service",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "EmployeeShifts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Service_ComboId",
                table: "Service",
                column: "ComboId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShifts_ScheduleId",
                table: "EmployeeShifts",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeShifts_Schedule_ScheduleId",
                table: "EmployeeShifts",
                column: "ScheduleId",
                principalTable: "Schedule",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Combo_ComboId",
                table: "Service",
                column: "ComboId",
                principalTable: "Combo",
                principalColumn: "Id");
        }
    }
}
