using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetGrooming_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class addScheduleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentService_AppointmentDetail_AppointmentDetailId",
                table: "AppointmentService");

            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentService_Service_ServiceId",
                table: "AppointmentService");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppointmentService",
                table: "AppointmentService");

            migrationBuilder.RenameTable(
                name: "AppointmentService",
                newName: "AppointmentServices");

            migrationBuilder.RenameIndex(
                name: "IX_AppointmentService_ServiceId",
                table: "AppointmentServices",
                newName: "IX_AppointmentServices_ServiceId");

            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "EmployeeShifts",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppointmentServices",
                table: "AppointmentServices",
                columns: new[] { "AppointmentDetailId", "ServiceId" });

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShifts_ScheduleId",
                table: "EmployeeShifts",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentServices_AppointmentDetail_AppointmentDetailId",
                table: "AppointmentServices",
                column: "AppointmentDetailId",
                principalTable: "AppointmentDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentServices_Service_ServiceId",
                table: "AppointmentServices",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeShifts_Schedule_ScheduleId",
                table: "EmployeeShifts",
                column: "ScheduleId",
                principalTable: "Schedule",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentServices_AppointmentDetail_AppointmentDetailId",
                table: "AppointmentServices");

            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentServices_Service_ServiceId",
                table: "AppointmentServices");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeShifts_Schedule_ScheduleId",
                table: "EmployeeShifts");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeShifts_ScheduleId",
                table: "EmployeeShifts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppointmentServices",
                table: "AppointmentServices");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "EmployeeShifts");

            migrationBuilder.RenameTable(
                name: "AppointmentServices",
                newName: "AppointmentService");

            migrationBuilder.RenameIndex(
                name: "IX_AppointmentServices_ServiceId",
                table: "AppointmentService",
                newName: "IX_AppointmentService_ServiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppointmentService",
                table: "AppointmentService",
                columns: new[] { "AppointmentDetailId", "ServiceId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentService_AppointmentDetail_AppointmentDetailId",
                table: "AppointmentService",
                column: "AppointmentDetailId",
                principalTable: "AppointmentDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentService_Service_ServiceId",
                table: "AppointmentService",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
