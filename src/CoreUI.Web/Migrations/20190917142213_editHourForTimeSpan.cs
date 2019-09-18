using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreUI.Web.Migrations
{
    public partial class editHourForTimeSpan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Total_Activies_Hours",
                table: "Hour",
                nullable: false,
                oldClrType: typeof(TimeSpan));

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Stop_Time_2",
                table: "Hour",
                nullable: false,
                oldClrType: typeof(TimeSpan));

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Stop_Time",
                table: "Hour",
                nullable: false,
                oldClrType: typeof(TimeSpan));

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Start_Time_2",
                table: "Hour",
                nullable: false,
                oldClrType: typeof(TimeSpan));

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Start_Time",
                table: "Hour",
                nullable: false,
                oldClrType: typeof(TimeSpan));

            migrationBuilder.AddColumn<DateTime>(
                name: "Arrival_Time",
                table: "Hour",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Beginning_Of_The_Break",
                table: "Hour",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "End_Of_The_Break",
                table: "Hour",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Exit_Time",
                table: "Hour",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Total_Hours_In_Activity",
                table: "Hour",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Arrival_Time",
                table: "Hour");

            migrationBuilder.DropColumn(
                name: "Beginning_Of_The_Break",
                table: "Hour");

            migrationBuilder.DropColumn(
                name: "End_Of_The_Break",
                table: "Hour");

            migrationBuilder.DropColumn(
                name: "Exit_Time",
                table: "Hour");

            migrationBuilder.DropColumn(
                name: "Total_Hours_In_Activity",
                table: "Hour");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Total_Activies_Hours",
                table: "Hour",
                nullable: false,
                oldClrType: typeof(TimeSpan));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Stop_Time_2",
                table: "Hour",
                nullable: false,
                oldClrType: typeof(TimeSpan));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Stop_Time",
                table: "Hour",
                nullable: false,
                oldClrType: typeof(TimeSpan));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Start_Time_2",
                table: "Hour",
                nullable: false,
                oldClrType: typeof(TimeSpan));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Start_Time",
                table: "Hour",
                nullable: false,
                oldClrType: typeof(TimeSpan));
           
        }
    }
}
