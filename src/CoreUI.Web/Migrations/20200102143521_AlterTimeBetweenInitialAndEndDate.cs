using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreUI.Web.Migrations
{
    public partial class AlterTimeBetweenInitialAndEndDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TimeBetweenInitialAndEndDate",
                table: "Pricing",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TimeBetweenInitialAndEndDate",
                table: "Pricing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
