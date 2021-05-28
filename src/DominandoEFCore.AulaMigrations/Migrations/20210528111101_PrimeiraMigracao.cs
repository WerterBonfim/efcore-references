using Microsoft.EntityFrameworkCore.Migrations;

namespace DominandoEFCore.AulaMigrations.Migrations
{
    public partial class PrimeiraMigracao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pessoas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pessoas", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pessoas");
        }
    }
}
