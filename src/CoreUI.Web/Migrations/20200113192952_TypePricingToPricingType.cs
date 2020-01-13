using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreUI.Web.Migrations
{
    public partial class TypePricingToPricingType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetailsPricing");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DetailsPricing",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AgeYears = table.Column<int>(nullable: false),
                    Cust = table.Column<double>(nullable: false),
                    DateBirth = table.Column<DateTime>(nullable: false),
                    HourConsultant = table.Column<double>(nullable: false),
                    HourSale = table.Column<double>(nullable: false),
                    HoursMonth = table.Column<int>(nullable: false),
                    Pricing_Id = table.Column<int>(nullable: false),
                    SpecialtyName = table.Column<string>(nullable: true),
                    TypeContract = table.Column<int>(nullable: false),
                    VT = table.Column<double>(nullable: false),
                    ValueCLTType = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailsPricing", x => x.Id);
                });
        }
    }
}
