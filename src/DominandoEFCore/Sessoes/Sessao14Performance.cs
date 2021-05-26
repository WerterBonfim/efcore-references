using System;
using System.Linq;
using DominandoEFCore.Data;
using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore.Sessoes
{
    public class Sessao14Performance
    {
        public static void ExecutarExemplos()
        {
            //SetupDaResoucaoDeIdentidade();
            //ConsultaRastreada();
            //ConsultaNaoRastreada();
            //ConsultaComResolucaoDeIdentidade();

            //ConsultaCustomizada();
            //ConsultaProjetadaERastreada();

            //SetupDaAulaConsultaProjetada();
            ConsultaProjetada();
        }

        private static void ConsultaProjetada()
        {
            //SemProjecao();

            ComProjecao();
        }

        private static void ComProjecao()
        {
            using var db = new ApplicationContext();

            var comProjecao = db.Departamentos
                .Select(x => x.Descricao)
                .ToArray();

            var memoriaComProjecao = (System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024) + " MB";
            Console.WriteLine($"Total de memoria utilizada: {memoriaComProjecao}");
        }

        private static void SemProjecao()
        {
            using var db = new ApplicationContext();

            var semProjecao = db.Departamentos.ToArray();

            var memoriaSemProjecao = (System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024) + " MB";
            Console.WriteLine($"Total de memoria utilizada: {memoriaSemProjecao}");
            
        }

        /// <summary>
        /// Vai inserir 200 departamentos com 1MB cada imagem
        /// </summary>
        private static void SetupDaAulaConsultaProjetada()
        {
            using var db = new ApplicationContext();
            Helpers.RecriarBancoDeDados(db);

            var random = new Random();
            
            db.Departamentos.AddRange(Enumerable.Range(1, 200).Select(x => new Departamento
            {
                Descricao = "Departamento Teste " + x,
                Imagem = GetBytes()
            }));

            db.SaveChanges();

            byte[] GetBytes()
            {
                var buffer = new byte[1024 * 1024];
                random.NextBytes(buffer);
                return buffer;
            }

        }

        private static void ConsultaProjetadaERastreada()
        {
            using var db = new ApplicationContext();

            var departamentos = db.Departamentos
                .Include(x => x.Funcionarios)
                .Select(x => new
                {
                    Departamento = x,
                    TotalFuncionarios = x.Funcionarios.Count()
                })
                .ToList();

            foreach (var departamento in departamentos)
                Console.WriteLine($"Departamento: {departamento.Departamento.Descricao} tem {departamento.TotalFuncionarios}");

            departamentos[0].Departamento.Descricao = "Departamento teste atualizado";

            db.SaveChanges();


        }

        private static void ConsultaCustomizada()
        {
            using var db = new ApplicationContext();

            // Em tempo de execução dessa instancia, irei rastrear todas as consultas
            db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            
            var funcionarios = db
                .Funcionarios
                .Include(x => x.Departamento)
                .ToList();
        }

        private static void ConsultaComResolucaoDeIdentidade()
        {
            using var db = new ApplicationContext();
            var funcionarios = db
                .Funcionarios
                .AsNoTrackingWithIdentityResolution()
                .Include(x => x.Departamento)
                .ToList();
        }

        private static void ConsultaRastreada()
        {
            using var db = new ApplicationContext();
            var funcionarios = db
                .Funcionarios
                .Include(x => x.Departamento)
                .ToList();
        }
        
        private static void ConsultaNaoRastreada()
        {
            using var db = new ApplicationContext();
            var funcionarios = db
                .Funcionarios
                .AsNoTracking()
                .Include(x => x.Departamento)
                .ToList();
        }

        static void SetupDaResoucaoDeIdentidade()
        {
            using var db = new ApplicationContext();
            Helpers.RecriarBancoDeDados(db);

            db.Departamentos.Add(new Departamento
            {
                Descricao = "Departamento de teste",
                Ativo = true,
                Funcionarios = Enumerable.Range(1, 100).Select(p => new Funcionario
                {
                    Cpf = p.ToString().PadLeft(11, '0'),
                    Nome = $"Funcionario {p}",
                    Rg = p.ToString()
                }).ToList()
            });

            db.SaveChanges();
        }
    }
}