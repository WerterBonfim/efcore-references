using System;
using System.Linq;
using DominandoEFCore.DicasTruques.Data;
using DominandoEFCore.DicasTruques.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace DominandoEFCore.DicasTruques
{
    class Program
    {
        static void Main(string[] args)
        {
            //ToQueryString();
            //DebugView();
            //Clear();
            //ConsultaFiltrada();
            //SingleOrDefaultVSFirstOrDefault();
            //SemChavePrimaria();
            //MontarSenarioViews();
            //ConfiguracaoVarchar();
            //OperadoresDeAgregacao();
            //OperadoresDeAgregacaoNoAgrupamento();
            ContadorDeEventos();
        }

        private static void ContadorDeEventos()
        {
            using var db = new ApplicationContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            Console.WriteLine($" PID: {System.Diagnostics.Process.GetCurrentProcess().Id}");

            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                var departamento = new Departamento
                {
                    Descricao = "Departamento Contador de evento"
                };

                db.Departamentos.Add(departamento);
                db.SaveChanges();


                _ = db.Departamentos.Find(1);
                _ = db.Departamentos.AsNoTracking().FirstOrDefault();
            }
        }

        private static void OperadoresDeAgregacaoNoAgrupamento()
        {
            using var db = new ApplicationContext();

            var sql = db.Departamentos
                .GroupBy(x => x.Descricao)
                .Where(x => x.Count() > 1)
                .Select(x => new
                {
                    Descricao = x.Key,
                    Contador = x.Count(),
                    Media = x.Average(y => y.Id),
                    Maximo = x.Max(y => y.Id),
                    Soma = x.Sum(y => y.Id)
                }).ToQueryString();

            Console.WriteLine(sql);
        }

        private static void OperadoresDeAgregacao()
        {
            using var db = new ApplicationContext();

            var sql = db.Departamentos
                .GroupBy(x => x.Descricao)
                .Select(x => new
                {
                    Descricao = x.Key,
                    Contador = x.Count(),
                    Media = x.Average(y => y.Id),
                    Maximo = x.Max(y => y.Id),
                    Soma = x.Sum(y => y.Id)
                }).ToQueryString();

            Console.WriteLine(sql);
        }

        private static void ConfiguracaoVarchar()
        {
            using var db = new ApplicationContext();
            var script = db.Database.GenerateCreateScript();

            Console.WriteLine(script);
        }

        private static void MontarSenarioViews()
        {
            using var db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Database.ExecuteSqlRaw(
                @"CREATE VIEW vw_departamento_relatorio AS
                SELECT
                    d.Descricao, COUNT(c.Id) AS Colaboradores
                FROM Departamentos AS d
           LEFT JOIN Colaboradores AS c ON c.DepartamentoId = d.Id
            GROUP BY d.Descricao");

            var departamentos = Enumerable.Range(1, 10)
                .Select(d => new Departamento
                {
                    Descricao = $"Departamento {d}",
                    Colaboradores = Enumerable.Range(1, d)
                        .Select(c => new Colaborador
                        {
                            Nome = $"Colaborador {d}-{c}"
                        }).ToList()
                }).ToList();

            db.Add(new Departamento {Descricao = "Departamento Sem Colaborador"});
            db.AddRange(departamentos);

            db.SaveChanges();

            var sql = db.DepartamentoRelatorios
                .Where(x => x.Colaboradores < 20)
                .ToQueryString();
            Console.WriteLine(sql);
            
            var relatorio = db.DepartamentoRelatorios
                .Where(x => x.Colaboradores < 20)
                .OrderBy(x => x.Departamento)
                .ToList();
            
            foreach (var dep in relatorio)
                Console.WriteLine($"{dep.Departamento} [ Colaboradores: {dep.Colaboradores}");
            
        }

        private static void SemChavePrimaria()
        {
            using var db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var usuarioFuncoes = db.UsuarioFuncaos
                .Where(x => x.UsuarioId == Guid.NewGuid())
                .ToQueryString();
            //.ToArray()

            Console.WriteLine(usuarioFuncoes);
        }

        private static void SingleOrDefaultVSFirstOrDefault()
        {
            using var db = new ApplicationContext();
        }

        private static void ConsultaFiltrada()
        {
            using var db = new ApplicationContext();

            var sql = db
                .Departamentos
                .Include(p => p.Colaboradores.Where(x => x.Nome.StartsWith("Werter")))
                .ToQueryString();

            Console.WriteLine(sql);
        }

        private static void Clear()
        {
            using var db = new ApplicationContext();

            db.Departamentos.Add(new Departamento {Descricao = "Teste DebugView"});

            db.ChangeTracker.Clear();
        }

        private static void DebugView()
        {
            using var db = new ApplicationContext();

            db.Departamentos.Add(new Departamento {Descricao = "Teste DebugView"});

            var query = db.Departamentos.Where(x => x.Id == 2);
        }

        static void ToQueryString()
        {
            using var db = new ApplicationContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var query = db.Departamentos.Where(x => x.Id == 2);

            var sql = query.ToQueryString();

            Console.WriteLine(sql);
        }
    }
}