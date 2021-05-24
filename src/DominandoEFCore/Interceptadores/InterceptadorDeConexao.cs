using System;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DominandoEFCore.Interceptadores
{
    public class InterceptadorDeConexao : DbConnectionInterceptor
    {
        public override InterceptionResult ConnectionOpening(
            DbConnection connection, 
            ConnectionEventData eventData, 
            InterceptionResult result)
        {

            Console.WriteLine("Entrei no metodo ConnectionOpening");

            Console.WriteLine(connection.ConnectionString);

            var builder = new SqlConnectionStringBuilder(connection.ConnectionString)
            {
                ApplicationName = "Rider CursoEFCore"
            };

            connection.ConnectionString = builder.ToString();
            Console.WriteLine(builder.ToString());
            
            return result;
        }
    }
}