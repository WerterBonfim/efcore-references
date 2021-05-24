using System;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DominandoEFCore.Interceptadores
{
    public class InterceptadorDeComandos : DbCommandInterceptor
    {

        private static readonly Regex _tableRegex =
            new Regex(@"(?<tableAlias>FROM +(\[.*\]\.)?(\[.*\]) AS (\[.*\])(?! WITH \(NOLOCK\)))", 
                RegexOptions.Multiline | 
                RegexOptions.IgnoreCase | 
                RegexOptions.Compiled);

    public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result)
        {
            Console.WriteLine("[Sync] Entrei dentro do metodo ReaderExecutings");
            UsarNoLock(command);

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

        private static void UsarNoLock(DbCommand command)
        {
            if (command.CommandText.Contains("WITH (NOLOCK)"))
                return;

            command.CommandText = _tableRegex
                .Replace(command.CommandText, "${tableAlias} WITH (NOLOCK)");
        }
    }
}