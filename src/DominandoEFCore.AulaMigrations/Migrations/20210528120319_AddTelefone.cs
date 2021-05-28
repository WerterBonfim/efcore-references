using Microsoft.EntityFrameworkCore.Migrations;

namespace DominandoEFCore.AulaMigrations.Migrations
{
    public partial class AddTelefone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Telefone",
                table: "Pessoas",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Telefone",
                table: "Pessoas");
        }
    }
}
