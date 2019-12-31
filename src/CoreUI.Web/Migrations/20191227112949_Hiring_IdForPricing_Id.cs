using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreUI.Web.Migrations
{
    public partial class Hiring_IdForPricing_Id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Hiring_Id",
                table: "DetailsPricing",
                newName: "Pricing_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Pricing_Id",
                table: "DetailsPricing",
                newName: "Hiring_Id");
        }
    }
}
