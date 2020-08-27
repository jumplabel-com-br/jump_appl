using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreUI.Web.Migrations
{
    public partial class AttachmentHours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Attachment",
                table: "Hour",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Email",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: true),
                    Remetente = table.Column<string>(nullable: true),
                    Telefone = table.Column<string>(nullable: true),
                    Mensagem = table.Column<string>(nullable: true),
                    Destino = table.Column<string>(nullable: true),
                    Assunto = table.Column<string>(nullable: true),
                    Empresa = table.Column<string>(nullable: true),
                    TipoCurso = table.Column<string>(nullable: true),
                    DataEnvio = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Email", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Email");

            migrationBuilder.DropColumn(
                name: "Attachment",
                table: "Hour");
        }
    }
}
