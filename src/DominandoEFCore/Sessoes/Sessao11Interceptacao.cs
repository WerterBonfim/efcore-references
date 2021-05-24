using System.Linq;
using DominandoEFCore.Data;

namespace DominandoEFCore.Sessoes
{
    public class Sessao11Interceptacao
    {
        public static void ExecutarExemplos()
        {
            TesteInterceptacao();
        }

        static void TesteInterceptacao()
        {
            using var db = new ApplicationContext();
            db.Funcoes.FirstOrDefault();
        }
        
    }
}