using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DominandoEFCore.Interceptadores
{
    public class InterceptadorDeComandos : DbCommandInterceptor
    {
        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result)
        {
            Console.WriteLine("[Sync] Entrei dentro do metodo ReaderExecutings");

            return base.ReaderExecuting(command, eventData, result);
        }

        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command, 
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            Console.WriteLine("[Async] Entrei dentro do metodo ReaderExecutings");
            return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }
    }
}