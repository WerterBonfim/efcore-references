using System;
using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace CursoEFCore
{
    class Program
    {
        
        

        static void Main(string[] args)
        {
            InserirDados();
            //InserirDadosEmMassa();
            //ConsultarDados();
            //CadastrarPedido();
            //ConsultarPedidoCarregamentoAdiantado();
            //AtualizarDados();
            //AtualizarDadosModoOffline();
            
            //RemoverRegistro();
            //RemoverRegistroDeFormaDesconectada();
        }

        private static void InserirDados()
        {

            var produto = new Produto
            {
                Descricao = "Produto teste",
                CodigoBarras = "12345512344",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            using var db = new Data.ApplicationContext();
            db.Produtos.Add(produto);
            //db.Set<Produto>().Add(produto);
            //db.Entry(produto).State = EntityState.Added;
            //db.Add(produto);

            var rowsAffeted = db.SaveChanges();

            Console.WriteLine($"Total Rows {rowsAffeted}");

        }

        private static void InserirDadosEmMassa()
        {
            var produto = new Produto
            {
                Descricao = "Produto teste",
                CodigoBarras = "12345512344",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            var cliente = new Cliente
            {
                Nome = "Werter Bonfim",
                CEP = "98765321",
                Cidade = "São Paulo",
                Estado = "SP",
                Telefone = "99987654321"
            };


            var clientes = new Cliente[] {
                new Cliente {
                    Nome = "Werter Bonfim",
                    CEP = "98765321",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    Telefone = "99987654321"
                },

                new Cliente {
                    Nome = "Fulano de tal",
                    CEP = "98765321",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    Telefone = "99987654321"
                },

            };


            using var db = new Data.ApplicationContext();
            db.AddRange(produto, cliente);



            db.AddRange(clientes);

            var registros = db.SaveChanges();
            System.Console.WriteLine($"Total registers: {registros}");

        }

        private static void ConsultarDados()
        {
            using var db = new Data.ApplicationContext();
            //var consultaPorSintaxe = (from c in db.Clientes where c.Id > 0 select c).ToList();
            //var consultaPorMetodo = db.Clientes.Where(p => p.Id > 0).ToList();
            var consultaPorMetodo = db.Clientes
                //.AsNoTracking()
                .Where(p => p.Id > 0)
                .OrderBy(x => x.Id)
                .ToList();


            foreach (var cliente in consultaPorMetodo)
            {
                System.Console.WriteLine($"Consultado o cliente {cliente.Id}");
                //var teste = db.Clientes.Find(cliente.Id);
                var teste = db.Clientes.FirstOrDefault(x => x.Id == cliente.Id);
                System.Console.WriteLine(teste.ToString());
            }
        }


        private static void CadastrarPedido()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido de teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrente.SemFrete,
                Itens = new List<PedidoItem> {
                    new PedidoItem{
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = 10
                    }
                }
            };

            db.Pedidos.Add(pedido);
            db.SaveChanges();
        }

        private static void ConsultarPedidoCarregamentoAdiantado(){
            using var db = new Data.ApplicationContext();
            var pedido = db.Pedidos
                .Include(p => p.Itens) // Carregamento adiantado
                    .ThenInclude(x => x.Produto)
                .ToList();
            //var pedido = db.Pedidos.Include("Itens").ToList();

            System.Console.WriteLine(pedido.Count);
        }


        private static void AtualizarDados(){
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.FirstOrDefault();
            cliente.Nome = "Cliente alterado 3";

            // com essa linha comentada, é gerado somente update set da coluna Nome
            //db.Clientes.Update(cliente); // altera todos todas as propriedade

            // outra forma de atualizar todas as propriedades
            //db.Entry(cliente).State = EntityState.Modified;

            db.SaveChanges();
        }

        private static void AtualizarDadosModoOffline(){
            using var db = new Data.ApplicationContext();

            var cliente = new Cliente {
                Id = 2
            };

            var clienteDesconectado = new {
                Id = 2,
                Nome = "Cliente desconectado 2",
                Telefone = "11998765432"
            };

            db.Attach(cliente);
            db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);

            db.SaveChanges();
        }


        private static void RemoverRegistro() {

            using var db = new Data.ApplicationContext();
            var cliente = db.Clientes.Find(3);

            db.Clientes.Remove(cliente);
            //db.Remove(cliente);
            //db.Entry(cliente).State = EntityState.Deleted;            
            db.SaveChanges();

        }

        private static void RemoverRegistroDeFormaDesconectada() {
            
            using var db = new Data.ApplicationContext();

            // Deleta de forma desconectada. Gera um query delete sem fazer select
            var cliente = new Cliente { Id = 4};
            db.Entry(cliente).State = EntityState.Deleted;

            db.SaveChanges();

        }

    }
}
