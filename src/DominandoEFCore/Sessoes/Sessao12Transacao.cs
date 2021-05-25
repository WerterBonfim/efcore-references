using System;
using System.Linq;
using System.Transactions;
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
            //SalvarPontoTransacao();
            ExemploTransactionScoped();
        }

        private static void ExemploTransactionScoped()
        {
            using var db = new ApplicationContext();
            Helpers.CadastrarLivro(db);

            var options = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            };

            using var scope = new TransactionScope(TransactionScopeOption.Required, options);
            ConsultarAtualizar();
            CadastrarLivroCodigoLimpo();
            CadastrarLivroDominandoEFCore();
            
            scope.Complete();
        }

        private static void CadastrarLivroDominandoEFCore()
        {
            using (var db = new ApplicationContext())
            {
                db.Livros.Add(new Livro
                {
                    Titulo = "Dominando o Entity Framework Core",
                    Autor = "Rafael Almeida"
                });
                db.SaveChanges();
            }
        }

        private static void CadastrarLivroCodigoLimpo()
        {
            using (var db = new ApplicationContext())
            {
                db.Livros.Add(new Livro
                {
                    Titulo = "Código Limpo",
                    Autor = "Robert C.Martin"
                });
                db.SaveChanges();
            }
        }

        private static void ConsultarAtualizar()
        {
            using (var db = new ApplicationContext())
            {
                var livro = db.Livros.FirstOrDefault(x => x.Id == 1);
                livro.Autor = "Werter to TDD2";
                db.SaveChanges();
            }
        }


        private static void SalvarPontoTransacao()
        {
            using var db = new ApplicationContext();
            Helpers.CadastrarLivro(db);

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
            Helpers.CadastrarLivro(db);

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
            Helpers.CadastrarLivro(db);

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
            Helpers.CadastrarLivro(db);

            var livro = db.Livros.FirstOrDefault(x => x.Id == 1);
            livro.Autor = "Werter Bonfim";

            db.Livros.Add(new Livro
            {
                Titulo = "Padrões e Praticas de Programação em C#",
                Autor = "Werter Bonfim"
            });

            db.SaveChanges();
        }

        
    }
}