using System;
using System.Collections.Generic;
using System.Linq;
using DominandoEFCore.Data;
using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore.Sessoes
{
    public class Sessao08ModeloDeDados
    {
        public static void Collections()
        {
            using var db = new ApplicationContext();

            Helpers.RecriarBancoDeDados(db);
        }

        public static void ProgagarDados()
        {
            using var db = new ApplicationContext();
            Helpers.RecriarBancoDeDados(db);

            var script = db.Database.GenerateCreateScript();
            Console.WriteLine(script);
        }

        public static void Esquema() => Helpers.ScriptEmConsole();
        public static void ConversorDeValor() => Helpers.ScriptEmConsole();

        public static void ConversorCustomizado()
        {
            using var db = new ApplicationContext();
            Helpers.RecriarBancoDeDados(db);

            db.Conversores.Add(
                new Conversor
                {
                    Status = Status.Devolvido
                });

            db.SaveChanges();

            var conversorEmAnalise = db.Conversores
                .AsNoTracking()
                .FirstOrDefault(p => p.Status == Status.Analise);

            var conversorDevolvido = db.Conversores
                .AsNoTracking()
                .FirstOrDefault(p => p.Status == Status.Devolvido);
        }

        public static void ShadowProperty() =>
            Helpers.ScriptEmConsole();

        public static void TrabalhandoComPropriedadesDeSombra()
        {
            using var db = new ApplicationContext();
            Helpers.RecriarBancoDeDados(db);

            var departamento = new Departamento
            {
                Descricao = "Departamento Propriedade de sombra"
            };

            db.Departamentos.Add(departamento);

            db.Entry(departamento).Property("UltimaAtualizacao").CurrentValue = DateTime.Now;

            db.SaveChanges();

            var departamentos = db.Departamentos
                .Where(p => EF.Property<DateTime>(p, "UltimaAtualizacao") < DateTime.Now)
                .ToArray();
            Console.WriteLine(departamento.Descricao);
        }

        // Muito bom
        public static void TiposDePropriedades()
        {
            using var db = new ApplicationContext();
            Helpers.RecriarBancoDeDados(db);

            var cliente = new Cliente
            {
                Nome = "Fulano de tal",

                Telefone = "(11) 98765-4321",
                Endereco = new Endereco {Bairro = "Centro", Cidade = "São Paulo"}
            };

            db.Clientes.Add(cliente);

            db.SaveChanges();

            var clientes = db.Clientes
                .AsNoTracking()
                .ToList();

            var options = new System.Text.Json.JsonSerializerOptions {WriteIndented = true};
            clientes.ForEach(cli =>
            {
                var json = System.Text.Json.JsonSerializer.Serialize(cli, options);
                Console.WriteLine(json);
            });
        }

        public static void Relacionamento1Para1()
        {
            using var db = new ApplicationContext();
            Helpers.RecriarBancoDeDados(db);

            var estado = new Estado
            {
                Nome = "São Paulo",
                Governador = new Governador {Nome = "Werter Bonfim"}
            };

            db.Estados.Add(estado);

            db.SaveChanges();

            var estados = db.Estados
                .AsNoTracking()
                .ToList();

            estados.ForEach(x => { Console.WriteLine($"Estado: {x.Nome}, Governador: {x.Governador.Nome}"); });
        }

        public static void Relacionamento1ParaMuitos()
        {
            using var db = new ApplicationContext();
            Helpers.RecriarBancoDeDados(db);

            var estado = new Estado
            {
                Nome = "São Paulo",
                Governador = new Governador {Nome = "Werter Bonfim"}
            };

            estado.Cidades.Add(new Cidade {Nome = "Mogi das Cruzes"});
            db.Estados.Add(estado);

            db.SaveChanges();

            var estados = db.Estados.ToList();
            estados[0].Cidades.Add(new Cidade {Nome = "Campinas"});

            db.SaveChanges();


            foreach (var est in db.Estados.Include(x => x.Cidades).AsNoTracking())
            {
                Console.WriteLine($"Estado {est.Nome}, Governador: {est.Governador.Nome}");
                foreach (var cidade in est.Cidades)
                    Console.WriteLine($"\t Cidade: {cidade.Nome}");
            }
        }

        public static void RelacionamentoMuitosParaMuitos()
        {
            using var db = new ApplicationContext();
            Helpers.RecriarBancoDeDados(db);

            var ator1 = new Ator {Nome = "Werter"};
            var ator2 = new Ator {Nome = "Fulano"};
            var ator3 = new Ator {Nome = "Beltrano"};

            var filme1 = new Filme {Descricao = "Os 7 samurais"};
            var filme2 = new Filme {Descricao = "O ultimo samurai"};
            var filme3 = new Filme {Descricao = "Zatoichi"};

            ator1.Filmes.Add(filme1);
            ator1.Filmes.Add(filme2);

            ator2.Filmes.Add(filme1);

            filme3.Atores.Add(ator1);
            filme3.Atores.Add(ator2);
            filme3.Atores.Add(ator3);

            db.AddRange(ator1, ator2, filme3);

            db.SaveChanges();

            foreach (var ator in db.Atores.Include(x => x.Filmes))
            {
                Console.WriteLine($"Ator: {ator.Nome}");

                foreach (var filme in ator.Filmes)
                {
                    Console.WriteLine($"\tFilme: {filme.Descricao}");
                }
            }
        }

        public static void CampoDeApoio()
        {
            using var db = new ApplicationContext();
            Helpers.RecriarBancoDeDados(db);

            var documento = new Documento();
            documento.DefinirCpf("12345678900");

            db.Documentos.Add(documento);
            db.SaveChanges();

            foreach (var doc in db.Documentos.AsNoTracking())
            {
                Console.WriteLine($"CPF -> {doc.CPF}");
            }
        }

        public static void ExemploTPH_TPT()
        {
            using var db = new ApplicationContext();
            Helpers.RecriarBancoDeDados(db);

            var pessoa = new Pessoa {Nome = "Fulano de tal"};
            var instrutor = new Instrutor {Nome = "Werter Bonfim", Tecnologia = "C#", Desde = DateTime.Now};
            var aluno = new Aluno {Nome = "Ciclano de tal", Idade = 23, DataContrato = DateTime.Now};

            db.AddRange(pessoa, instrutor, aluno);
            db.SaveChanges();

            var pessoas = db.Pessoas.AsNoTracking().ToArray();
            var instrutores = db.Instrutores.AsNoTracking().ToArray();
            var alunos = db.Alunos.AsNoTracking().ToArray();
            //var alunos = db.Pessoas.OfType<Aluno>().AsNoTracking().ToArray();

            Console.WriteLine($"Pessoas ================================================");
            foreach (var x in pessoas)
                Console.WriteLine(x.ToString());

            Console.WriteLine($"Instrutores ================================================");
            foreach (var x in instrutores)
                Console.WriteLine(x.ToString());

            Console.WriteLine($"Alunos ================================================");
            foreach (var x in alunos)
                Console.WriteLine(x.ToString());
        }

        public static void SacolaDePropriedades()
        {
            using var db = new ApplicationContext();
            Helpers.RecriarBancoDeDados(db);

            var configuracao = new Dictionary<string, object>
            {
                ["Chave"] = "SenhaBancoDeDados",
                ["Valor"] = Guid.NewGuid().ToString()
            };

            db.Configuracoes.Add(configuracao);
            db.SaveChanges();

            var configuracoes = db
                .Configuracoes
                .AsNoTracking()
                .Where(p => p["Chave"] == "SenhaBancoDeDados")
                .ToArray();

            foreach (var c in configuracoes)
                Console.WriteLine($"Chave: {c["Chave"]} - Valor: {c["Valor"]}");
        }
    }
}