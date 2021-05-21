using System;
using System.Linq;
using DominandoEFCore.Data;
using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore.Sessoes
{
    public class Sessao10EFFunctions
    {
        public static void ExecutarExemplos()
        {
            FuncoesDeDatas();
        }

        static void FuncoesDeDatas()
        {
            using var db = new ApplicationContext();
            ApagarERecriarBancoDeDados(db);
            Helpers.ScriptEmConsole();
            
            

            var dados = db.Funcoes
                .AsNoTracking()
                .Select(x => new
                {
                    
                    Dias = EF.Functions.DateDiffDay(DateTime.Now, x.Data1),
                    Data = EF.Functions.DateFromParts(2021, 5, 21),
                    DataValida = EF.Functions.IsDate(x.Data2)
                });

            foreach (var d in dados)
                Console.WriteLine(d);
            
        }

        static void ApagarERecriarBancoDeDados(ApplicationContext db)
        {
            Helpers.RecriarBancoDeDados(db);
            Console.Clear();

            db.Funcoes.AddRange(
                new Funcao
                {
                    Data1 = DateTime.Now.AddDays(2),
                    Data2 = "2021-05-20",
                    Descricao1 = "Bala 1",
                    Descricao2 = "Bala 1"
                },
                new Funcao
                {
                    Data1 = DateTime.Now.AddDays(1),
                    Data2 = "XX21-05-01",
                    Descricao1 = "Bala 2",
                    Descricao2 = "Bala 2"
                },
                new Funcao
                {
                    Data1 = DateTime.Now.AddDays(3),
                    Data2 = "XX21-05-20",
                    Descricao1 = "Bala 3",
                    Descricao2 = "Bala 3"
                }
            );

            db.SaveChanges();
        }
    }
}