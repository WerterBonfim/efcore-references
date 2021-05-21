using System;
using System.Linq;
using DominandoEFCore.Data;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore.Sessoes
{
    public class Sessao04TiposDeCarregamento
    {
        public static void CarregamentoAdiantado()
        {
            using var db = new ApplicationContext();
            Helpers.CargaInicial(db);

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

        public static void CarregamentoExplicito()
        {
            using var db = new ApplicationContext();
            Helpers.CargaInicial(db);

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

        public static void CarregamentoLento()
        {
            using var db = new ApplicationContext();
            Helpers.CargaInicial(db);

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
    }
}