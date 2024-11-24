using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetGrooming_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class fixdatabase_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentDetail_Appointment_AppointmentId",
                table: "AppointmentDetail");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentDetail_AppointmentId",
                table: "AppointmentDetail");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentDetail_ComboId",
                table: "AppointmentDetail");

            migrationBuilder.DropColumn(
                name: "IsWorking",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WorkStatus",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "AppointmentId",
                table: "AppointmentDetail",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_AppointmentId",
                table: "AppointmentDetail",
                column: "AppointmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_ComboId",
                table: "AppointmentDetail",
                column: "ComboId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentDetail_Appointment_AppointmentId",
                table: "AppointmentDetail",
                column: "AppointmentId",
                principalTable: "Appointment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentDetail_Appointment_AppointmentId",
                table: "AppointmentDetail");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentDetail_AppointmentId",
                table: "AppointmentDetail");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentDetail_ComboId",
                table: "AppointmentDetail");

            migrationBuilder.AddColumn<bool>(
                name: "IsWorking",
                table: "Users",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WorkStatus",
                table: "Users",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "AppointmentId",
                table: "AppointmentDetail",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_AppointmentId",
                table: "AppointmentDetail",
                column: "AppointmentId",
                unique: true,
                filter: "[AppointmentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_ComboId",
                table: "AppointmentDetail",
                column: "ComboId",
                unique: true,
                filter: "[ComboId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentDetail_Appointment_AppointmentId",
                table: "AppointmentDetail",
                column: "AppointmentId",
                principalTable: "Appointment",
                principalColumn: "Id");
        }
    }
}
