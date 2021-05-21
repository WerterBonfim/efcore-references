using System;
using DominandoEFCore.Data;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore.Sessoes
{
    public class Sessao09DataAnnotations
    {
        public static void ExecutarExemplos()
        {
            Atributos();
        }

        static void Atributos()
        {
            using var db = new ApplicationContext();
            var script = db.Database.GenerateCreateScript();
            Console.WriteLine(script);
        }
    }
}