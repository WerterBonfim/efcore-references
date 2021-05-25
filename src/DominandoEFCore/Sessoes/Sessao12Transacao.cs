using System;
using System.Linq;
using DominandoEFCore.Data;
using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore.Sessoes
{
    public class Sessao12Transacao
    {
        public static void ExecutarExemplos()
        {
            //ComportamentoPadrao();
            //GerenciandoTransacaoManualmente();
            //RevertendoTransacao();
            SalvarPontoTransacao();
        }

        private static void SalvarPontoTransacao()
        {
            using var db = new ApplicationContext();
            CadastrarLivro(db);

            var transacao = db.Database.BeginTransaction();
            try
            {
                var livro = db.Livros.FirstOrDefault(x => x.Id == 1);
                livro.Autor = "Werter do TDD";
                db.SaveChanges();
                
                transacao.CreateSavepoint("desfazer_apenas_insercao");

                Console.ReadKey();

                db.Livros.Add(new Livro
                {
                    Titulo = "Boas praticas e código limpo",
                    Autor = "Werter"
                });

                db.SaveChanges();

                // Da um erro aqui
                db.Livros.Add(new Livro
                {
                    Titulo = "Qualquer um",
                    Autor = "Vai dar erro".PadLeft(16, '*')
                });
                
                db.SaveChanges();
            
                transacao.Commit();
            }
            catch (DbUpdateException e)
            {
                transacao.RollbackToSavepoint("desfazer_apenas_insercao");

                var todasEntidadesSaoDeInclusao = 
                    e.Entries
                        .Count(x => x.State == EntityState.Added) == e.Entries.Count;
                
                if (todasEntidadesSaoDeInclusao) transacao.Commit();
                
            }
        }

        private static void RevertendoTransacao()
        {
            using var db = new ApplicationContext();
            CadastrarLivro(db);

            var transacao = db.Database.BeginTransaction();
            try
            {
                var livro = db.Livros.FirstOrDefault(x => x.Id == 1);
                livro.Autor = "Werter TDD".PadLeft(16, '*');
                db.SaveChanges();

                Console.ReadKey();
            

                db.Livros.Add(new Livro
                {
                    Titulo = "Boas praticas e código limpo",
                    Autor = "Werter"
                });

                db.SaveChanges();
            
                transacao.Commit();
            }
            catch (Exception e)
            {
                transacao.Rollback();
            }

            
        }

        private static void GerenciandoTransacaoManualmente()
        {
            using var db = new ApplicationContext();
            CadastrarLivro(db);

            var transacao = db.Database.BeginTransaction();

            var livro = db.Livros.FirstOrDefault(x => x.Id == 1);
            livro.Autor = "Werter TDD";
            db.SaveChanges();

            Console.ReadKey();
            

            db.Livros.Add(new Livro
            {
                Titulo = "Boas praticas e código limpo",
                Autor = "Werter"
            });

            db.SaveChanges();
            
            transacao.Commit();
        }

        private static void ComportamentoPadrao()
        {
            using var db = new ApplicationContext();
            CadastrarLivro(db);

            var livro = db.Livros.FirstOrDefault(x => x.Id == 1);
            livro.Autor = "Werter Bonfim";

            db.Livros.Add(new Livro
            {
                Titulo = "Padrões e Praticas de Programação em C#",
                Autor = "Werter Bonfim"
            });

            db.SaveChanges();
        }

        private static void CadastrarLivro(ApplicationContext db)
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