using System;
using System.Linq;
using DominandoEFCore.Data;

namespace DominandoEFCore.Sessoes
{
    public class Sessao13UDFs
    {
        public static void ExecutarExemplos()
        {
            FunscaoLeft();
        }

        private static void FunscaoLeft()
        {
            using var db = new ApplicationContext();
            Helpers.CadastrarLivro(db);

            var resultado = db.Livros
                .Select(x => ApplicationContext.Left(x.Titulo, 5));
            
            foreach(var tituloLivro in resultado)
                Console.WriteLine(tituloLivro);
        }
    }
}