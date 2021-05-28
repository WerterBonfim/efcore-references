using System;
using System.Linq;
using DominandoEFCore.DemaisBancos.Data;

namespace DominandoEFCore.DemaisBancos
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