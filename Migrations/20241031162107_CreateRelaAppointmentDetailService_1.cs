using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetGrooming_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class CreateRelaAppointmentDetailService_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Service_AppointmentDetail_AppointmentDetailId",
                table: "Service");

            migrationBuilder.DropIndex(
                name: "IX_Service_AppointmentDetailId",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "AppointmentDetailId",
                table: "Service");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "StartTime",
                table: "Shift",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "EndTime",
                table: "Shift",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time");

            migrationBuilder.CreateTable(
                name: "AppointmentService",
                columns: table => new
                {
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    AppointmentDetailId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentService", x => new { x.AppointmentDetailId, x.ServiceId });
                    table.ForeignKey(
                        name: "FK_AppointmentService_AppointmentDetail_AppointmentDetailId",
                        column: x => x.AppointmentDetailId,
                        principalTable: "AppointmentDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentService_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentService_ServiceId",
                table: "AppointmentService",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentService");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "StartTime",
                table: "Shift",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0),
                oldClrType: typeof(TimeOnly),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "EndTime",
                table: "Shift",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0),
                oldClrType: typeof(TimeOnly),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppointmentDetailId",
                table: "Service",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Service_AppointmentDetailId",
                table: "Service",
                column: "AppointmentDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Service_AppointmentDetail_AppointmentDetailId",
                table: "Service",
                column: "AppointmentDetailId",
                principalTable: "AppointmentDetail",
                principalColumn: "Id");
        }
    }
}
