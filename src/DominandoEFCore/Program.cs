﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DominandoEFCore.Data;
using DominandoEFCore.Domain;
using Microsoft.Data.SqlClient;
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
            //ScriptGeralBancoDeDados();

            // Sessão 04 - tipos de carregamento
            //CarregamentoAdiantado();
            //CarregamentoExplicito();
            //CarregamentoLento();
            
            // Sessão 05 - Consultas
            //IgnoreFiltroGlobal();
            //ConsultasProjetadas();
            //ConsultaParmetrizada();
            //ConsultaInterpolada();
            //ConsultaComTAG();
            //EntendendoConsultas1NN1();
            //DivisaoDeConsulta();
            
            
            // Sessão 06 - Stored Procedure
            //CriarStoredProcedure();
            //InserirDadosViaProcedure();
            //CriarStoredProcedureDeConsulta();
            ConsultaViaProcedure();
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

        static void CarregamentoAdiantado()
        {
            using var db = new ApplicationContext();
            CargaInicial(db);

            var departamentos = db
                .Departamentos
                .Include(p => p.Funcionarios);

            foreach (var departamento in departamentos)
            {
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine($"Departamento: {departamento.Descricao}");

                if (departamento.Funcionarios.Any())
                    foreach (var funcionario in departamento.Funcionarios)
                        Console.WriteLine($"\tFuncionario: {funcionario.Nome}");
                else
                    Console.WriteLine($"\tNenhum funcionario encontrado!");
            }
        }

        static void CarregamentoExplicito()
        {
            using var db = new ApplicationContext();
            CargaInicial(db);

            var departamentos = db
                // Se não for chamado o .ToList() 
                // a conexão fica aberta, pode gerar um erro.
                // Com ToList os dados são buscados do banco de dados
                // e o EF fecha a conexão.
                // Outra alternativa e utilizar a propriedade
                // MultipleActiveResultSets=true na string de conexão             
                .Departamentos 
                .ToList();


            foreach (var departamento in departamentos)
            {
                if (departamento.Id == 2)
                {
                    db.Entry(departamento)
                        //.Collection("Funcionarios")
                        .Collection(p => p.Funcionarios)
                        // .Load(); // Carrega todos os funcionarios do departamento
                        .Query()
                        // Efetua um filtro em cima dos funcionario do departamento.
                        // Uma maneira de aplicar as regras de negocio 
                        .Where(p => p.Id > 2)
                        .ToList();
                }

                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine($"Departamento: {departamento.Descricao}");

                if (departamento.Funcionarios?.Any() ?? false)
                    foreach (var funcionario in departamento.Funcionarios)
                        Console.WriteLine($"\tFuncionario: {funcionario.Nome}");
                else
                    Console.WriteLine($"\tNenhum funcionario encontrado!");
            }
        }

        static void CarregamentoLento()
        {
            using var db = new ApplicationContext();
            CargaInicial(db);

            //Desablitar
            //db.ChangeTracker.LazyLoadingEnabled = false;

            var departamentos = db
                .Departamentos 
                .ToList();


            foreach (var departamento in departamentos)
            {
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine($"Departamento: {departamento.Descricao}");

                if (departamento.Funcionarios?.Any() ?? false)
                    foreach (var funcionario in departamento.Funcionarios)
                        Console.WriteLine($"\tFuncionario: {funcionario.Nome}");
                else
                    Console.WriteLine($"\tNenhum funcionario encontrado!");
            }
        }

        

        #region [ Helpers ]
        private static void CargaInicial(ApplicationContext db)
        {
            RecriarBancoDeDados(db);

            db.Departamentos.AddRange(
                new Departamento
                {
                    Descricao = "Departamento 01",
                    Funcionarios = new List<Funcionario>
                    {
                        new Funcionario {Nome = "Werter Bonfim", Cpf = "82159287067", Rg = "1245678985"}
                    },
                    Excluido = true
                },
                new Departamento
                {
                    Descricao = "Departamento 02",
                    Funcionarios = new List<Funcionario>
                    {
                        new Funcionario {Nome = "Fulano de tal", Cpf = "37075422030", Rg = "213156453"},
                        new Funcionario {Nome = "Ciclano de tal", Cpf = "66606840007", Rg = "372172702"}
                    }
                }
            );

            db.SaveChanges();
            db.ChangeTracker.Clear();
        }

        private static void RecriarBancoDeDados(ApplicationContext db)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
        

        #endregion

        #region [ 05 - Consultas ]

        static void FiltroGlobal()
        {
            using var db = new ApplicationContext();
            //CargaInicial(db);

            var departamentos = db.Departamentos
                .Where(p => p.Id > 0)
                .ToList();

            foreach (var departamento in departamentos)
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluido: {departamento.Excluido}");
            
        }
        static void IgnoreFiltroGlobal()
        {
            using var db = new ApplicationContext();
            //CargaInicial(db);

            var departamentos = db.Departamentos
                .IgnoreQueryFilters()
                .Where(p => p.Id > 0)
                .ToList();

            foreach (var departamento in departamentos)
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluido: {departamento.Excluido}");
            
        }
        
        static void ConsultasProjetadas()
        {
            using var db = new ApplicationContext();
            //CargaInicial(db);

            var departamentos = db.Departamentos
                .Where(p => p.Id > 0)
                .Select(p => new
                {
                    p.Descricao,
                    Funcionarios = p.Funcionarios
                        .Select(f => f.Nome)
                })
                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Departamento: {departamento.Descricao}");
                foreach(var funcionario in departamento.Funcionarios)
                    Console.WriteLine($"\tFuncionario: {funcionario}");
            }

            
        }
        
        static void ConsultaParmetrizada()
        {
            using var db = new ApplicationContext();
            //CargaInicial(db);

            var id = 0;

            var departamentos = db.Departamentos
                .FromSqlRaw("SELECT * FROM Departamentos WITH(NOLOCK) WHERE id > {0}", id)
                .Where(p => !p.Excluido)
                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Departamento: {departamento.Descricao}");
                
            }
        }
        
        static void ConsultaInterpolada()
        {
            using var db = new ApplicationContext();
            //CargaInicial(db);

            var id = 0;

            var departamentos = db.Departamentos
                .FromSqlInterpolated($"SELECT * FROM Departamentos WITH(NOLOCK) WHERE id > {id}" )
                .Where(p => !p.Excluido)
                .ToList();

            foreach (var departamento in departamentos)
                Console.WriteLine($"Departamento: {departamento.Descricao}");

        }
        
        static void ConsultaComTAG()
        {
            using var db = new ApplicationContext();
            //CargaInicial(db);

            var id = 0;

            var departamentos = db.Departamentos
                .TagWith(@"Estou enviando um comentário para o servidor
        
                        segundo comentário qualquer
                        terceiro comentário ")
                .ToList();

            foreach (var departamento in departamentos)
                Console.WriteLine($"Departamento: {departamento.Descricao}");

        }
        
        static void EntendendoConsultas1NN1()
        {
            using var db = new ApplicationContext();

            var departamentos = db.Departamentos
                .Include(p => p.Funcionarios)
                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Departamento: {departamento.Descricao}");
                foreach (var funcionario in departamento.Funcionarios)
                {
                    Console.WriteLine($"\tNome: {funcionario.Nome}");
                }
            }


            var funcionarios = db.Funcionarios
                .Include(p => p.Departamento)
                .ToList();

            foreach (var funcionario in funcionarios)
                Console.WriteLine($"Nome: {funcionario.Nome}\tDepartamento: {funcionario.Departamento.Descricao}");


        }
        
        static void DivisaoDeConsulta()
        {
            using var db = new ApplicationContext();

            var departamentos = db.Departamentos
                .Include(p => p.Funcionarios)
                .Where(p => p.Id < 3)
                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Departamento: {departamento.Descricao}");
                foreach (var funcionario in departamento.Funcionarios)
                {
                    Console.WriteLine($"\tNome: {funcionario.Nome}");
                }
            }
            
            
            var departamentos2 = db.Departamentos
                .Include(p => p.Funcionarios)
                .Where(p => p.Id < 3)
                //.AsSplitQuery()
                //.AsSingleQuery()
                .ToList();

            foreach (var departamento in departamentos2)
            {
                Console.WriteLine($"Departamento: {departamento.Descricao}");
                foreach (var funcionario in departamento.Funcionarios)
                {
                    Console.WriteLine($"\tNome: {funcionario.Nome}");
                }
            }

           


        }
        
        #endregion

        #region [ 07 - Stored Procedure ]

        static void CriarStoredProcedure()
        {
            var criarDepartamento = @"
                    create or
                    alter procedure CriarDepartamento @Descricao varchar(50),
                                                      @Ativo bit
                    as
                    begin
                        insert into Departamentos (Descricao, Ativo, Excluido)
                        values (@Descricao, @Ativo, 0)
                    end";

            using var db = new ApplicationContext();

            db.Database.ExecuteSqlRaw(criarDepartamento);
        }

        static void InserirDadosViaProcedure()
        {
            using var db = new ApplicationContext();

            //db.Database.ExecuteSqlRaw("execute CriarDepartamento @p0, @p1", new object[] {"", ""});
            db.Database.ExecuteSqlRaw("execute CriarDepartamento @p0, @p1", 
                "Departamento Via Procedure", true);

        }
        static void CriarStoredProcedureDeConsulta()
        {
            var criarPesquisa = @"
                    create or
                    alter procedure ListarDepartamentos @Descricao varchar(50)
                    as
                    begin
                        select * from Departamentos where Descricao like @Descricao + '%'
                    end";
            
            using var db = new ApplicationContext();
            db.Database.ExecuteSqlRaw(criarPesquisa);

        }
        
        static void ConsultaViaProcedure()
        {
            using var db = new ApplicationContext();

            var dep = new SqlParameter("@dep", "departamento");

            var departamentos = db.Departamentos
                // Por debaixo dos panos ele gerar um @p0
                //.FromSqlRaw("execute ListarDepartamentos {0}", "dep")
                //.FromSqlRaw("execute ListarDepartamentos @dep", dep)
                //.FromSqlInterpolated($"execute ListarDepartamentos {dep}")
                .FromSqlRaw("execute ListarDepartamentos @p0", "dep")
                .ToList();

            foreach (var departamento in departamentos)
                Console.WriteLine($"Nome departamento: {departamento.Descricao}");
            

        }
        
        #endregion
        
    }
}