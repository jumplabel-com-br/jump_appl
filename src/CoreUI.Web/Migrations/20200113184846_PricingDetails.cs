using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreUI.Web.Migrations
{
    public partial class PricingDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PricingDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TypeContract = table.Column<int>(nullable: false),
                    Pricing_Id = table.Column<int>(nullable: false),
                    SpecialtyName = table.Column<string>(nullable: true),
                    HoursMonth = table.Column<int>(nullable: false),
                    HourConsultant = table.Column<double>(nullable: false),
                    HourSale = table.Column<double>(nullable: false),
                    ValueCLTType = table.Column<double>(nullable: false),
                    VT = table.Column<double>(nullable: false),
                    Cust = table.Column<double>(nullable: false),
                    DateBirth = table.Column<DateTime>(nullable: false),
                    AgeYears = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricingDetails", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PricingDetails");
        }
    }
}
