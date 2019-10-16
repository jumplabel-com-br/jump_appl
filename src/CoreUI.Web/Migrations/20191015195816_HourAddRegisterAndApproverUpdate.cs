using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreUI.Web.Migrations
{
    public partial class HourAddRegisterAndApproverUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Approver_Update",
                table: "Hour",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Register",
                table: "Hour",
                nullable: true,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approver_Update",
                table: "Hour");

            migrationBuilder.DropColumn(
                name: "Register",
                table: "Hour");
        }
    }
}
