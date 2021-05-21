using System;
using System.Linq;
using DominandoEFCore.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore.Sessoes
{
    public class Sessao06StoredProcedure
    {
        public static void CriarStoredProcedure()
        {
            var criarDepartamento = @"
                    create or
                    alter procedure CriarDepartamento @Descricao varchar(50),
                                                      @Ativo bit
                    as
                    begin
                        insert into Departamentos (Descricao, Ativo, Excluido)
                        values (@Descricao, @Ativo, 0)
                    end";

            using var db = new ApplicationContext();

            db.Database.ExecuteSqlRaw(criarDepartamento);
        }

        public static void InserirDadosViaProcedure()
        {
            using var db = new ApplicationContext();

            //db.Database.ExecuteSqlRaw("execute CriarDepartamento @p0, @p1", new object[] {"", ""});
            db.Database.ExecuteSqlRaw("execute CriarDepartamento @p0, @p1",
                "Departamento Via Procedure", true);
        }

        public static void CriarStoredProcedureDeConsulta()
        {
            var criarPesquisa = @"
                    create or
                    alter procedure ListarDepartamentos @Descricao varchar(50)
                    as
                    begin
                        select * from Departamentos where Descricao like @Descricao + '%'
                    end";

            using var db = new ApplicationContext();
            db.Database.ExecuteSqlRaw(criarPesquisa);
        }

        public static void ConsultaViaProcedure()
        {
            using var db = new ApplicationContext();

            var dep = new SqlParameter("@dep", "departamento");

            var departamentos = db.Departamentos
                // Por debaixo dos panos ele gerar um @p0
                //.FromSqlRaw("execute ListarDepartamentos {0}", "dep")
                //.FromSqlRaw("execute ListarDepartamentos @dep", dep)
                //.FromSqlInterpolated($"execute ListarDepartamentos {dep}")
                .FromSqlRaw("execute ListarDepartamentos @p0", "dep")
                .ToList();

            foreach (var departamento in departamentos)
                Console.WriteLine($"Nome departamento: {departamento.Descricao}");
        }
    }
}