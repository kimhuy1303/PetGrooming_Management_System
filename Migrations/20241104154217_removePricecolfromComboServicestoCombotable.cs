using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetGrooming_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class removePricecolfromComboServicestoCombotable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "ComboServices");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Combo",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Combo");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "ComboServices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
