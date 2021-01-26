using Microsoft.EntityFrameworkCore.Migrations;

namespace MariaBonitaAPI.Data.Migrations
{
    public partial class NumeroAndComplementoAddedToEndereco1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                table: "Enderecos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Numero",
                table: "Enderecos",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Complemento",
                table: "Enderecos");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Enderecos");
        }
    }
}
