using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreUI.Web.Migrations
{
    public partial class Outlays : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Outlays",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Employee_Id = table.Column<int>(nullable: false),
                    Project_Id = table.Column<int>(nullable: false),
                    Client_Id = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    NoteNumber = table.Column<string>(nullable: true),
                    NoteValue = table.Column<double>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    File = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outlays", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Outlays");
        }
    }
}
