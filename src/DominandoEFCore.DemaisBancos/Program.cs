using System;
using System.Linq;
using DominandoEFCore.DemaisBancos.Data;
using DominandoEFCore.DemaisBancos.Domain;

namespace DominandoEFCore.DemaisBancos
{
    public class Program
    {
        static void Main(string[] args)
        {
            using var db = new ApplicationContext();

            db.Database.EnsureCreated();

            db.Pessoas.Add(new Pessoa
            {
                Nome = "Werter",
                Telefone = "1198765432"
            });

            db.SaveChanges();

            var pessoas = db.Pessoas.ToList();
            Console.WriteLine($"Foram encontrados {pessoas.Count}");
        }
    }
}