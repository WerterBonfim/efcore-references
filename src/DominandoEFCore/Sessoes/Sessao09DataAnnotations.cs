using System;
using DominandoEFCore.Data;
using DominandoEFCore.Domain;
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
            Helpers.RecriarBancoDeDados(db);

            Console.WriteLine(db.Database.GenerateCreateScript());

            db.Atributos.Add(new Atributo
            {
                Descricao = "Exemplo",
                Observacao = "Observação"
            });

            db.SaveChanges();

        }
    }
}