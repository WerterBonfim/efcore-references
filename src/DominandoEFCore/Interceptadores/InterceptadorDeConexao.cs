using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
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

            Console.WriteLine("[Sync] Entrei no metodo ConnectionOpening");

            Console.WriteLine(connection.ConnectionString);

            var builder = new SqlConnectionStringBuilder(connection.ConnectionString)
            {
                ApplicationName = "Rider CursoEFCore",
                
            };

            connection.ConnectionString = builder.ToString();
            
            Console.WriteLine(builder.ToString());
            
            return result;
        }

        public override ValueTask<InterceptionResult> ConnectionOpeningAsync(DbConnection connection, ConnectionEventData eventData, InterceptionResult result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            
            Console.WriteLine("[Async] Entrei no metodo ConnectionOpening");

            Console.WriteLine(connection.ConnectionString);

            var builder = new SqlConnectionStringBuilder(connection.ConnectionString);

            connection.ConnectionString = builder.ToString();
            Console.WriteLine(builder.ToString());
            
            return base.ConnectionOpeningAsync(connection, eventData, result, cancellationToken);
        }
    }
}