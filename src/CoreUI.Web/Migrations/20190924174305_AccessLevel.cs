using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreUI.Web.Migrations
{
    public partial class AccessLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Access_LevelId",
                table: "Employee",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Access_Level",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Access_level = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Access_Level", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Access_LevelId",
                table: "Employee",
                column: "Access_LevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Access_Level_Access_LevelId",
                table: "Employee",
                column: "Access_LevelId",
                principalTable: "Access_Level",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Access_Level_Access_LevelId",
                table: "Employee");

            migrationBuilder.DropTable(
                name: "Access_Level");

            migrationBuilder.DropIndex(
                name: "IX_Employee_Access_LevelId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Access_LevelId",
                table: "Employee");
        }
    }
}
