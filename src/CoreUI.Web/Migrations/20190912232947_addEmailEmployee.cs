using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreUI.Web.Migrations
{
    public partial class addEmailEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmailEmployeeId",
                table: "Hour",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Employee_Id",
                table: "Hour",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HourId",
                table: "Employee",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hour_EmailEmployeeId",
                table: "Hour",
                column: "EmailEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_HourId",
                table: "Employee",
                column: "HourId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Hour_HourId",
                table: "Employee",
                column: "HourId",
                principalTable: "Hour",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Hour_Employee_EmailEmployeeId",
                table: "Hour",
                column: "EmailEmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Hour_HourId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Hour_Employee_EmailEmployeeId",
                table: "Hour");

            migrationBuilder.DropIndex(
                name: "IX_Hour_EmailEmployeeId",
                table: "Hour");

            migrationBuilder.DropIndex(
                name: "IX_Employee_HourId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "EmailEmployeeId",
                table: "Hour");

            migrationBuilder.DropColumn(
                name: "Employee_Id",
                table: "Hour");

            migrationBuilder.DropColumn(
                name: "HourId",
                table: "Employee");
        }
    }
}