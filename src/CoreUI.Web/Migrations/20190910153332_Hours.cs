using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreUI.Web.Migrations
{
    public partial class Hours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hour",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Project = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Start_Time = table.Column<DateTime>(nullable: false),
                    Stop_Time = table.Column<DateTime>(nullable: false),
                    Start_Time_2 = table.Column<DateTime>(nullable: false),
                    Stop_Time_2 = table.Column<DateTime>(nullable: false),
                    Activies = table.Column<string>(nullable: true),
                    Total_Activies_Hours = table.Column<string>(nullable: true),
                    Consultant = table.Column<string>(nullable: true),
                    Creation_Date = table.Column<DateTime>(nullable: false),
                    Id_Project = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hour", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hour");
        }
    }
}