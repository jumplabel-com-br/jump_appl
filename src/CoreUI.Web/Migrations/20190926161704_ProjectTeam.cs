using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreUI.Web.Migrations
{
    public partial class ProjectTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Project_team",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Project_Id = table.Column<int>(nullable: false),
                    Employee_Id = table.Column<int>(nullable: false),
                    Start_Date = table.Column<DateTime>(nullable: false),
                    End_Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_team", x => x.Id);
                });

            /*
            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Access_Level_Access_LevelId",
                table: "Employee",
                column: "Access_LevelId",
                principalTable: "Access_Level",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
                */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Access_Level_Access_LevelId",
                table: "Employee");
            */

            migrationBuilder.DropTable(
                name: "Project_team");
        }
    }
}
