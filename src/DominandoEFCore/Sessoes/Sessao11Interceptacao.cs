using System.Linq;
using DominandoEFCore.Data;
using DominandoEFCore.Domain;

namespace DominandoEFCore.Sessoes
{
    public class Sessao11Interceptacao
    {
        public static void ExecutarExemplos()
        {
            //TesteInterceptacao();
            TesteInterceptacaoSaveChanges();
        }

        private static void TesteInterceptacaoSaveChanges()
        {
            using var db = new ApplicationContext();
            Helpers.RecriarBancoDeDados(db);

            db.Funcoes.Add(new Funcao
            {
                Descricao1 = "Teste"
            });

            db.SaveChanges();
        }

        static void TesteInterceptacao()
        {
            using var db = new ApplicationContext();
            db.Funcoes.FirstOrDefault();
        }
        
    }
}