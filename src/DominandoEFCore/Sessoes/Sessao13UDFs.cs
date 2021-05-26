using System;
using System.Linq;
using DominandoEFCore.Data;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore.Sessoes
{
    public class Sessao13UDFs
    {
        public static void ExecutarExemplos()
        {
            //FunscaoLeft();
            //FuncaoDefinidaPeloUsuario();
            DateDIFF();
        }

        private static void DateDIFF()
        {
            using var db = new ApplicationContext();
            Helpers.CadastrarLivro(db);

            var resultado = db
                .Livros
                .Select(x => SqlHelperFunctions.DateDiff("DAY", x.CadastradoEm, DateTime.Now));
            
            foreach (var diferenca in resultado)
                Console.WriteLine($"A diferença em dias é: {diferenca}");
            
        }

        private static void FuncaoDefinidaPeloUsuario()
        {
            using var db = new ApplicationContext();
            Helpers.CadastrarLivro(db);

            db.Database.ExecuteSqlRaw(@"
                CREATE FUNCTION ConverterParaLetrasMaiusculas(@dados VARCHAR(100))
                RETURNS VARCHAR(100)
                BEGIN
                    RETURN UPPER(@dados)
                END
            ");

            var resultado = db.Livros
                .Select(x => SqlHelperFunctions.LetrasMaiusculas(x.Titulo));

            foreach (var titulo in resultado)
                Console.WriteLine(titulo);
            
        }

        private static void FunscaoLeft()
        {
            using var db = new ApplicationContext();
            Helpers.CadastrarLivro(db);

            var resultado = db.Livros
                .Select(x => SqlHelperFunctions.Left(x.Titulo, 5));
            
            foreach(var tituloLivro in resultado)
                Console.WriteLine(tituloLivro);
        }
    }
}