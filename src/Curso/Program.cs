using System;
using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CursoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            //InserirDados();
            InserirDadosEmMassa();
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
            db.Set<Produto>().Add(produto);
            db.Entry(produto).State = EntityState.Added;
            db.Add(produto);

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

    }
}
