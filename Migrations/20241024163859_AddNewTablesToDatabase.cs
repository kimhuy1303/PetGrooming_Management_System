using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetGrooming_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class AddNewTablesToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id_Shift",
                table: "Shift",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Id_Schedule",
                table: "Schedule",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Id_Appointment",
                table: "Appointment",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Appointment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Appointment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdCustomer",
                table: "Appointment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Annoucement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Annoucement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Combo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Combo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAnnouncements",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AnnoucementId = table.Column<int>(type: "int", nullable: false),
                    HasRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAnnouncements", x => new { x.UserId, x.AnnoucementId });
                    table.ForeignKey(
                        name: "FK_UserAnnouncements_Annoucement_AnnoucementId",
                        column: x => x.AnnoucementId,
                        principalTable: "Annoucement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAnnouncements_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeWorking = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppointmentId = table.Column<int>(type: "int", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    ComboId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppointmentDetail_Appointment_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppointmentDetail_Combo_ComboId",
                        column: x => x.ComboId,
                        principalTable: "Combo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppointmentDetail_Users_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PetName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WeightPet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    AppointmentDetailId = table.Column<int>(type: "int", nullable: true),
                    ComboId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Service_AppointmentDetail_AppointmentDetailId",
                        column: x => x.AppointmentDetailId,
                        principalTable: "AppointmentDetail",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Service_Combo_ComboId",
                        column: x => x.ComboId,
                        principalTable: "Combo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ComboServices",
                columns: table => new
                {
                    ComboId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComboServices", x => new { x.ServiceId, x.ComboId });
                    table.ForeignKey(
                        name: "FK_ComboServices_Combo_ComboId",
                        column: x => x.ComboId,
                        principalTable: "Combo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComboServices_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_CustomerId",
                table: "Appointment",
                column: "CustomerId");

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

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_EmployeeId",
                table: "AppointmentDetail",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboServices_ComboId",
                table: "ComboServices",
                column: "ComboId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_AppointmentDetailId",
                table: "Service",
                column: "AppointmentDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_ComboId",
                table: "Service",
                column: "ComboId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnnouncements_AnnoucementId",
                table: "UserAnnouncements",
                column: "AnnoucementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Users_CustomerId",
                table: "Appointment",
                column: "CustomerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Users_CustomerId",
                table: "Appointment");

            migrationBuilder.DropTable(
                name: "ComboServices");

            migrationBuilder.DropTable(
                name: "UserAnnouncements");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "Annoucement");

            migrationBuilder.DropTable(
                name: "AppointmentDetail");

            migrationBuilder.DropTable(
                name: "Combo");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_CustomerId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "IdCustomer",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Shift",
                newName: "Id_Shift");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Schedule",
                newName: "Id_Schedule");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Appointment",
                newName: "Id_Appointment");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Appointment",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
