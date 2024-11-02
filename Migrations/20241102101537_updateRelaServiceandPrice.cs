using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetGrooming_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class updateRelaServiceandPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Price_ServiceId",
                table: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Price_ServiceId",
                table: "Price",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Price_ServiceId",
                table: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Price_ServiceId",
                table: "Price",
                column: "ServiceId",
                unique: true,
                filter: "[ServiceId] IS NOT NULL");
        }
    }
}
