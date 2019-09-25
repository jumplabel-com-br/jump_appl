using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreUI.Web.Migrations
{
    public partial class removeSalary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salary",
                table: "Employee");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Employee",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Employee",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<double>(
                name: "Salary",
                table: "Employee",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
