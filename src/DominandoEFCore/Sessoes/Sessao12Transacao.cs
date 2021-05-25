using System;
using System.Linq;
using DominandoEFCore.Data;
using DominandoEFCore.Domain;

namespace DominandoEFCore.Sessoes
{
    public class Sessao12Transacao
    {
        public static void ExecutarExemplos()
        {
            //ComportamentoPadrao();
            GerenciandoTransacaoManualmente();
        }

        private static void GerenciandoTransacaoManualmente()
        {
            using var db = new ApplicationContext();
            CadastrarLivro(db);

            var transacao = db.Database.BeginTransaction();

            var livro = db.Livros.FirstOrDefault(x => x.Id == 1);
            livro.Autor = "Werter TDD";
            db.SaveChanges();

            //Console.ReadKey();
            

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