using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreUI.Web.Migrations
{
    public partial class DeleteColumnsHour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "HourId",
                table: "Employee");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Total_Activies_Hours",
                table: "Hour",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Employee",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Employee",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Total_Activies_Hours",
                table: "Hour",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<int>(
                name: "EmailEmployeeId",
                table: "Hour",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Employee",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Employee",
                nullable: true,
                oldClrType: typeof(string));

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
    }
}