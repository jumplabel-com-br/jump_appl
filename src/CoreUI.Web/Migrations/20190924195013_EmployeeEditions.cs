using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreUI.Web.Migrations
{
    public partial class EmployeeEditions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_Client_ClientId",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_ClientId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Project");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Project",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_ClientId",
                table: "Project",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Client_ClientId",
                table: "Project",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
