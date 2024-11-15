using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetGrooming_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class fixdatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Annoucement");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Annoucement");

            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "UserAnnouncements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnnoucementId",
                table: "Appointment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAnnouncements_AppointmentId",
                table: "UserAnnouncements",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_AnnoucementId",
                table: "Appointment",
                column: "AnnoucementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Annoucement_AnnoucementId",
                table: "Appointment",
                column: "AnnoucementId",
                principalTable: "Annoucement",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnnouncements_Appointment_AppointmentId",
                table: "UserAnnouncements",
                column: "AppointmentId",
                principalTable: "Appointment",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Annoucement_AnnoucementId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnnouncements_Appointment_AppointmentId",
                table: "UserAnnouncements");

            migrationBuilder.DropIndex(
                name: "IX_UserAnnouncements_AppointmentId",
                table: "UserAnnouncements");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_AnnoucementId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "UserAnnouncements");

            migrationBuilder.DropColumn(
                name: "AnnoucementId",
                table: "Appointment");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Annoucement",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Annoucement",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
