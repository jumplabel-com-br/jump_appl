using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreUI.Web.Migrations
{
    public partial class addTypeReleases : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeReleases",
                table: "Employee",
                nullable: false,
                defaultValue: 0);

            /*migrationBuilder.CreateTable(
                name: "ListHourAPI",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Projeto = table.Column<string>(nullable: true),
                    Clientes = table.Column<string>(nullable: true),
                    Data = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    HoraInicio = table.Column<TimeSpan>(nullable: false),
                    Pausa = table.Column<TimeSpan>(nullable: false),
                    Retorno = table.Column<TimeSpan>(nullable: false),
                    HoraFim = table.Column<TimeSpan>(nullable: false),
                    Atividades = table.Column<string>(nullable: true),
                    TotalHoras = table.Column<TimeSpan>(nullable: false),
                    Consultor = table.Column<string>(nullable: true),
                    Senha = table.Column<string>(nullable: true),
                    Cobranca = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListHourAPI", x => x.Id);
                });*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DropTable(
                name: "ListHourAPI");
                */
            migrationBuilder.DropColumn(
                name: "TypeReleases",
                table: "Employee");
        }
    }
}
