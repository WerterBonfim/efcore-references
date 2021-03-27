using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CursoEFCore.Migrations
{
    public partial class PrimeiraMigracao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Clientes",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>("VARCHAR(80)", nullable: false),
                    Telefone = table.Column<string>("CHAR(11)", nullable: false),
                    CEP = table.Column<string>("CHAR(80)", nullable: false),
                    Estado = table.Column<string>("CHAR(2)", nullable: false),
                    Cidade = table.Column<string>("nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Clientes", x => x.Id); });

            migrationBuilder.CreateTable(
                "Produtos",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoBarras = table.Column<string>("VARCHAR(14)", nullable: false),
                    Descricao = table.Column<string>("VARCHAR(60)", nullable: true),
                    Valor = table.Column<decimal>("decimal(18,2)", nullable: false),
                    TipoProduto = table.Column<string>("nvarchar(max)", nullable: false),
                    Ativo = table.Column<bool>("bit", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Produtos", x => x.Id); });

            migrationBuilder.CreateTable(
                "Pedidos",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>("int", nullable: false),
                    IniciadoEm = table.Column<DateTime>("datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    FinalizadoEm = table.Column<DateTime>("datetime2", nullable: false),
                    TipoFrete = table.Column<int>("int", nullable: false),
                    Status = table.Column<string>("nvarchar(max)", nullable: false),
                    Observacao = table.Column<string>("varchar(512)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                    table.ForeignKey(
                        "FK_Pedidos_Clientes_ClienteId",
                        x => x.ClienteId,
                        "Clientes",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "PedidoItens",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PedidoId = table.Column<int>("int", nullable: false),
                    ProdutoId = table.Column<int>("int", nullable: false),
                    Quantidade = table.Column<int>("int", nullable: false, defaultValue: 1),
                    Valor = table.Column<decimal>("decimal(18,2)", nullable: false),
                    Desconto = table.Column<decimal>("decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoItens", x => x.Id);
                    table.ForeignKey(
                        "FK_PedidoItens_Pedidos_PedidoId",
                        x => x.PedidoId,
                        "Pedidos",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_PedidoItens_Produtos_ProdutoId",
                        x => x.ProdutoId,
                        "Produtos",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "idx_cliente_telefone",
                "Clientes",
                "Telefone");

            migrationBuilder.CreateIndex(
                "IX_PedidoItens_PedidoId",
                "PedidoItens",
                "PedidoId");

            migrationBuilder.CreateIndex(
                "IX_PedidoItens_ProdutoId",
                "PedidoItens",
                "ProdutoId");

            migrationBuilder.CreateIndex(
                "IX_Pedidos_ClienteId",
                "Pedidos",
                "ClienteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "PedidoItens");

            migrationBuilder.DropTable(
                "Pedidos");

            migrationBuilder.DropTable(
                "Produtos");

            migrationBuilder.DropTable(
                "Clientes");
        }
    }
}