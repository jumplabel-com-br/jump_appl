using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreUI.Web.Migrations
{
    public partial class EditAccountExecutiveInPricing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountExecutive",
                table: "Pricing");

            migrationBuilder.AddColumn<int>(
                name: "AccountExecutive_Id",
                table: "Pricing",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountExecutive_Id",
                table: "Pricing");

            migrationBuilder.AddColumn<string>(
                name: "AccountExecutive",
                table: "Pricing",
                nullable: true);
        }
    }
}
