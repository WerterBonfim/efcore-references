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
            InserirDados();
        }

        private static void InserirDados(){
            
            var produto = new Produto {
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
    }
}
