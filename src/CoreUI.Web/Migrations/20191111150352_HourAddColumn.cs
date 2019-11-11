using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreUI.Web.Migrations
{
    public partial class HourAddColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Description",
                table: "Hour",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "File",
                table: "Hour",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Hour");

            migrationBuilder.DropColumn(
                name: "File",
                table: "Hour");
        }
    }
}
