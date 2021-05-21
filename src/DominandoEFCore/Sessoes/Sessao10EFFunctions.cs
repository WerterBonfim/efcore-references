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
            //FuncoesDeDatas();
            //FuncaoLike();
            //FuncaoDataLength();
            //FuncaoProperty();
            FuncaoCollate();
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
                    Descricao2 = "Tela"
                },
                new Funcao
                {
                    Data1 = DateTime.Now.AddDays(1),
                    Data2 = "XX21-05-01",
                    Descricao1 = "Bala 2",
                    Descricao2 = "Bola 2"
                },
                new Funcao
                {
                    Data1 = DateTime.Now.AddDays(3),
                    Data2 = "XX21-05-20",
                    Descricao1 = "Biruta 3",
                    Descricao2 = "Tela"
                }
            );

            db.SaveChanges();
        }

        static void FuncaoLike()
        {
            using var db = new ApplicationContext();
            ApagarERecriarBancoDeDados(db);

            var dados = db
                .Funcoes
                .AsNoTracking()
                .Where(x => EF.Functions.Like(x.Descricao1, "B[ao]%"))
                .Select(x => x.Descricao1)
                .ToArray();

            Console.WriteLine("Resultado: ");
            foreach (var descricao in dados)
                Console.WriteLine(descricao);
            

        }

        static void FuncaoDataLength()
        {
            using var db = new ApplicationContext();
            ApagarERecriarBancoDeDados(db);

            var resultado = db
                .Funcoes
                .AsNoTracking()
                .Select(x => new
                {
                    TotalBytesCampoData = EF.Functions.DataLength(x.Data1),
                    TotalBytes1 = EF.Functions.DataLength(x.Descricao1),
                    TotalBytes2 = EF.Functions.DataLength(x.Descricao2),
                    Total1 = x.Descricao1.Length,
                    Total2 = x.Descricao2.Length
                })
                .FirstOrDefault();

            Console.WriteLine("Resultado:");
            Console.WriteLine(resultado);
        }

        static void FuncaoProperty()
        {
            using var db = new ApplicationContext();
            ApagarERecriarBancoDeDados(db);

            // As propriedades de sombra não existe no tipo CLR da entidade Funcao.
            // As propriedade de sobra são somente associadas a cada registro se a  
            // consulta for rastreada
            
            var resultado = db
                .Funcoes
                //.AsNoTracking()
                .FirstOrDefault(x => EF.Property<string>(x, "PropriedadeSombra") == "Teste");

            var propriedadeSombra = db
                .Entry(resultado)
                .Property<string>("PropriedadeSombra")
                .CurrentValue;

            Console.WriteLine("Resultado:");
            Console.WriteLine(propriedadeSombra);
        }

        static void FuncaoCollate()
        {
            using var db = new ApplicationContext();
            ApagarERecriarBancoDeDados(db);
            
            var caseSensitive = "SQL_Latin1_General_CP1_CS_AS";
            var caseInsensitive = "SQL_Latin1_General_CP1_CI_AS";

            var consultaCS = db
                .Funcoes
                .FirstOrDefault(x => EF.Functions.Collate(x.Descricao2, caseSensitive) == "tela");
            
            var consultaCI = db
                .Funcoes
                .FirstOrDefault(x => EF.Functions.Collate(x.Descricao2, caseInsensitive) == "tela");

            Console.WriteLine($"Consulta 1 CS: {consultaCS?.Descricao2}");
            Console.WriteLine($"Consulta 2 CI: {consultaCI?.Descricao2}");


        }
        
    }
}