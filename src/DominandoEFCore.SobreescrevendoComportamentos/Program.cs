using System;
using System.Linq;
using DominandoEFCore.SobreescrevendoComportamentos.Data;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore.SobreescrevendoComportamentos
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Exemplo();
        }

        private static void Exemplo()
        {
            using var db = new ApplicationContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var sql = db
                .Departamentos
                .Where(x => x.Id > 0)
                .ToQueryString();

            Console.WriteLine(sql);
        }
    }
}