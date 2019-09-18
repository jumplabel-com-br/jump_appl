using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreUI.Web.Migrations
{
    public partial class Employee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "Employee",
               columns: table => new
               {
                   Id = table.Column<int>(nullable: false)
                       .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                   Email = table.Column<string>(nullable: false),
                   Name = table.Column<string>(nullable: true),
                   Document = table.Column<string>(nullable: true),
                   Contract_Mode = table.Column<string>(nullable: true),
                   Active = table.Column<int>(nullable: false),
                   Salary = table.Column<double>(nullable: false),
                   Appoiment = table.Column<int>(nullable: false),
                   Password = table.Column<string>(nullable: false),
                   Change_Password = table.Column<int>(nullable: false)

               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_Employee", x => x.Id);
               });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}