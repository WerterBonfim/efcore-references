using System.Linq;
using DominandoEFCore.Data;
using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore.Sessoes
{
    public class Sessao07Infraestrutura
    {
        public static void ConsultarDepartamentos()
        {
            using var db = new ApplicationContext();

            var departamentos = db.Departamentos
                .Where(p => p.Id > 0)
                .ToArray();
        }

        public static void DadosSensiveis()
        {
            using var db = new ApplicationContext();

            var departamento = "Departamento";

            var departamentos = db.Departamentos
                .Where(x => x.Descricao == departamento)
                .ToArray();
        }

        public static void HabilitandoBatchSize()
        {
            using var db = new ApplicationContext();

            Helpers.RecriarBancoDeDados(db);

            for (var i = 0; i < 50; i++)
                db.Departamentos.Add(new Departamento {Descricao = "Departamento " + i});

            db.SaveChanges();
        }

        public static void TempoCommandoGeral()
        {
            using var db = new ApplicationContext();

            db.Database.ExecuteSqlRaw("WAITFOR DELAY '00:00:07'  ;SELECT 1");
        }

        public static void TempoCommandoGeral2()
        {
            using var db = new ApplicationContext();

            db.Database.SetCommandTimeout(10);
            db.Database.ExecuteSqlRaw("WAITFOR DELAY '00:00:07'  ;SELECT 1");
        }

        public static void ExecutarEstrategiaResiliencia()
        {
            using var db = new ApplicationContext();

            var strategy = db.Database.CreateExecutionStrategy();
            strategy.Execute(() =>
            {
                using var transaction = db.Database.BeginTransaction();

                db.Departamentos.Add(new Departamento {Descricao = "Departament transação"});
                db.SaveChanges();

                transaction.Commit();
            });
        }
    }
}