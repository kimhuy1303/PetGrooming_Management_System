using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetGrooming_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class fixdatabase_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PetName",
                table: "AppointmentServices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PetWeight",
                table: "AppointmentServices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "AppointmentServices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "PetName",
                table: "AppointmentDetail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PetWeight",
                table: "AppointmentDetail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PetName",
                table: "AppointmentServices");

            migrationBuilder.DropColumn(
                name: "PetWeight",
                table: "AppointmentServices");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "AppointmentServices");

            migrationBuilder.DropColumn(
                name: "PetName",
                table: "AppointmentDetail");

            migrationBuilder.DropColumn(
                name: "PetWeight",
                table: "AppointmentDetail");
        }
    }
}
