using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetGrooming_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class fixComboServicesTable_02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Combo");

            migrationBuilder.AddColumn<string>(
                name: "PetName",
                table: "ComboServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PetWeight",
                table: "ComboServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "ComboServices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PetName",
                table: "ComboServices");

            migrationBuilder.DropColumn(
                name: "PetWeight",
                table: "ComboServices");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ComboServices");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Combo",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
