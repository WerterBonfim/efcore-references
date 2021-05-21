using System;
using System.Linq;
using DominandoEFCore.Data;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore.Sessoes
{
    public class Sessao05Consultas
    {
        public static void FiltroGlobal()
        {
            using var db = new ApplicationContext();
            //CargaInicial(db);

            var departamentos = db.Departamentos
                .Where(p => p.Id > 0)
                .ToList();

            foreach (var departamento in departamentos)
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluido: {departamento.Excluido}");
        }

        public static void IgnoreFiltroGlobal()
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

        public static void ConsultasProjetadas()
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
                foreach (var funcionario in departamento.Funcionarios)
                    Console.WriteLine($"\tFuncionario: {funcionario}");
            }
        }

        public static void ConsultaParmetrizada()
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

        public static void ConsultaInterpolada()
        {
            using var db = new ApplicationContext();
            //CargaInicial(db);

            var id = 0;

            var departamentos = db.Departamentos
                .FromSqlInterpolated($"SELECT * FROM Departamentos WITH(NOLOCK) WHERE id > {id}")
                .Where(p => !p.Excluido)
                .ToList();

            foreach (var departamento in departamentos)
                Console.WriteLine($"Departamento: {departamento.Descricao}");
        }

        public static void ConsultaComTAG()
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

        public static void EntendendoConsultas1NN1()
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

        public static void DivisaoDeConsulta()
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
    }
}