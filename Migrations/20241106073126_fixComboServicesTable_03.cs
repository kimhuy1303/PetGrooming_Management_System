using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetGrooming_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class fixComboServicesTable_03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ComboServices",
                table: "ComboServices");

            migrationBuilder.DropIndex(
                name: "IX_ComboServices_ComboId",
                table: "ComboServices");

            migrationBuilder.AlterColumn<string>(
                name: "PetWeight",
                table: "ComboServices",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PetName",
                table: "ComboServices",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComboServices",
                table: "ComboServices",
                columns: new[] { "ComboId", "ServiceId", "PetName", "PetWeight" });

            migrationBuilder.CreateIndex(
                name: "IX_ComboServices_ServiceId",
                table: "ComboServices",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ComboServices",
                table: "ComboServices");

            migrationBuilder.DropIndex(
                name: "IX_ComboServices_ServiceId",
                table: "ComboServices");

            migrationBuilder.AlterColumn<string>(
                name: "PetWeight",
                table: "ComboServices",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "PetName",
                table: "ComboServices",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComboServices",
                table: "ComboServices",
                columns: new[] { "ServiceId", "ComboId" });

            migrationBuilder.CreateIndex(
                name: "IX_ComboServices_ComboId",
                table: "ComboServices",
                column: "ComboId");
        }
    }
}
