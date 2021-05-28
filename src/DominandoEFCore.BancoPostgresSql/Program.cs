using System;
using System.Linq;
using DominandoEFCore.BancoPostgresSql.Data;

namespace DominandoEFCore.BancoPostgresSql
{
    public class Program
    {
        static void Main(string[] args)
        {
            using var db = new ApplicationContext();

            db.Database.EnsureCreated();

            var pessoas = db.Pessoas.ToList();
            Console.WriteLine($"Foram encontrados {pessoas.Count}");
        }
    }
}