using System;
using System.Diagnostics;
using System.Linq;
using DominandoEFCore.Data;
using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace DominandoEFCore
{
    internal class Program
    {
        private static int _count;

        private static void Main(string[] args)
        {
            //EnsureCreatedAndDeleted();
            //GapDoEnsureCreated();
            //HealthCheckBancoDeDados();

            //BenchmarkRunner.Run<Monitoramento>();

            //GerenciamentoConexao();

            //ExecuteSQL();
            //SqlInjection();
            
            //MigracoesPendentes();
            //AplicarMigraçãoEmTempoDeExecução();
            //TodasMigracoes();
            //MigracoesJaAplicadas();
            ScriptGeralBancoDeDados();
        }

        private static void GerenciamentoConexao()
        {
            // warmup
            new ApplicationContext().Departamentos.AsNoTracking().Any();
            _count = 0;
            GerenciarEstadoDaConexao(false);
            _count = 0;
            GerenciarEstadoDaConexao(true);
        }

        private static void EnsureCreatedAndDeleted()
        {
            using var db = new ApplicationContext();
            //db.Database.EnsureCreated();
            db.Database.EnsureDeleted();
        }

        private static void GapDoEnsureCreated()
        {
            using var db1 = new ApplicationContext();
            using var db2 = new ApplicationContextCidade();

            db1.Database.EnsureCreated();
            db2.Database.EnsureCreated();

            var databaseCreator = db2.GetService<IRelationalDatabaseCreator>();
            databaseCreator.CreateTables();
        }

        private static void HealthCheckBancoDeDados()
        {
            using var db = new ApplicationContext();
            var possoConectar = db.Database.CanConnect();
            if (possoConectar) Console.WriteLine("Banco online");
            else Console.WriteLine("Banco não esta disponível");
        }
        private static void GerenciarEstadoDaConexao(bool gerenciarEstadoConexao)
        {
            using var db = new ApplicationContext();
            var time = Stopwatch.StartNew();

            var conexao = db.Database.GetDbConnection();

            conexao.StateChange += (_, __) => ++_count;
            if (gerenciarEstadoConexao)
                conexao.Open();

            for (var i = 0; i < 200; i++)
                db.Departamentos.AsNoTracking().Any();

            time.Stop();
            var mensagem = $"Tempo: {time.Elapsed.ToString()}, {gerenciarEstadoConexao}, Contador: {_count}";
            Console.WriteLine(mensagem);
        }

        static void ExecuteSQL()
        {
            using var db = new ApplicationContext();
            RestarBaseDeDados(db);

            //Primeira opção
            // using (var cmd = db.Database.GetDbConnection().CreateCommand())
            // {
            //     cmd.CommandText = "SELECT 1";
            //     cmd.ExecuteNonQuery();
            // }

            // segunda opção
            var injection = "Teste ' or 1='1";
            var descricao = "TEste";
            db.Database.ExecuteSqlRaw("update departamentos set descricao={0} where id = 1", injection);

            // terceira opção
            db.Database.ExecuteSqlInterpolated($"update departamentos set descricao={injection} where id = 1");
        }

        static void SqlInjection()
        {
            using var db = new ApplicationContext();
            RestarBaseDeDados(db);

            var injection = "Teste ' or 1='1";
            var query = "update departamentos set descricao = 'AtaqueSqlInjection' " + 
                        $"where descricao = '{injection}'";
            
            db.Database.ExecuteSqlRaw(query);
            foreach (var departamento in db.Departamentos.AsNoTracking())
            {
                Console.WriteLine($"Id: {departamento.Id}, Descrição: {departamento.Descricao}");
            }
        }
        private static void RestarBaseDeDados(ApplicationContext db)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            
            db.Departamentos.AddRange(
                new Departamento {Descricao = "Departamento 1"},
                new Departamento {Descricao = "Departamento 2"},
                new Departamento {Descricao = "Departamento 3"}
            );

            db.SaveChanges();
        }

        static void MigracoesPendentes()
        {
            using var db = new ApplicationContext();

            var migracoesPendentes = db.Database.GetPendingMigrations();
            Console.WriteLine($"Total: {migracoesPendentes.Count()}");

            foreach (var migracoes in migracoesPendentes)
                Console.WriteLine($"Migração: {migracoes}");
            
        }

        static void AplicarMigraçãoEmTempoDeExecução()
        {
            using var db = new ApplicationContext();
            db.Database.Migrate();
        }

        static void TodasMigracoes()
        {
            using var db = new ApplicationContext();
            var migracoes = db.Database.GetMigrations();

            Console.WriteLine($"Total: {migracoes.Count()}");

            foreach (var migracao in migracoes)
                Console.WriteLine($"Migração: {migracao}");
            
        }
        
        static void MigracoesJaAplicadas()
        {
            using var db = new ApplicationContext();
            var migracoes = db.Database.GetAppliedMigrations();

            Console.WriteLine("Migrações já aplicadas:");
            Console.WriteLine($"Total: {migracoes.Count()}");

            foreach (var migracao in migracoes)
                Console.WriteLine($"Migração: {migracao}");
            
        }

        static void ScriptGeralBancoDeDados()
        {
            using var db = new ApplicationContext();
            var script = db.Database.GenerateCreateScript();

            Console.WriteLine(script);
        }
    }
}