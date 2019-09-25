using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreUI.Web.Migrations
{
    public partial class Project_Team : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Project_team",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(nullable: true),
                    Project_Id = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: true),
                    Employee_Id = table.Column<int>(nullable: false),
                    Start_Date = table.Column<DateTime>(nullable: false),
                    End_Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_team", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Project_team_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_team_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Project_team_EmployeeId",
                table: "Project_team",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_team_ProjectId",
                table: "Project_team",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Project_team");
        }
    }
}
