using System;
using System.Collections.Generic;
using DominandoEFCore.Data;
using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore
{
    public class Helpers
    {
        public static void CargaInicial(ApplicationContext db)
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

        public static void RecriarBancoDeDados(ApplicationContext db)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }

        public static void ScriptEmConsole()
        {
            using var db = new ApplicationContext();
            var script = db.Database.GenerateCreateScript();
            Console.WriteLine(script);
        }
        
        public static void CadastrarLivro(ApplicationContext db)
        {
            Helpers.RecriarBancoDeDados(db);

            db.Livros.Add(new Livro
            {
                Titulo = "A Arte de Escrever Programas Legíveis",
                Autor = "Dustin Boswell"
            });

            db.SaveChanges();
        }
    }
}