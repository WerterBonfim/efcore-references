# Interceptação

* 1 - O que são interceptadores de comandos?
* 2 - Criando e registrando um interceptador
* 3 - Sobrescrevendo métodos da classe base
* 4 - Aplicando hint NOLOCK nas consultas
* 5 - Interceptando abertura de conexão com o banco
* 6 - Interceptando alterações.



## 1 - O que são interceptadores de comandos?
Três interfaces importantes para esse recurso:
    * IDbCommandInterceptor
    * IDbConnectionInterceptor
    * IDbTransactionInterceptor


## 2 - Criando e registrando um interceptador
```c#

public class InterceptadorDeComandos : DbCommandInterceptor
{    
}

// Definindo um interceptador


//OnConfiguring
optionsBuilder
    .UseSqlServer(stringDeConexao)
    .EnableSensitiveDataLogging()
    .LogTo(Console.WriteLine, LogLevel.Information)
    .AddInterceptors(new InterceptadorDeComandos())

```


## 3 - Sobrescrevendo métodos da classe base

[Docs oficial sobre interceptação][doc-interceptacao]




[doc-interceptacao]:https://docs.microsoft.com/en-us/ef/core/logging-events-diagnostics/interceptors

## 4 - Aplicando hint NOLOCK nas consultas

Um exemplo pratico

 ```c#

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

private static void UsarNoLock(DbCommand command)
{
    if (command.CommandText.Contains("WITH (NOLOCK)"))
        return;

    command.CommandText = _tableRegex
        .Replace(command.CommandText, "${tableAlias} WITH (NOLOCK)");
}
 ```



## 5 - Interceptando abertura de conexão com o banco

```c#
optionsBuilder
    .UseSqlServer(stringDeConexao)
    .EnableSensitiveDataLogging()
    .LogTo(Console.WriteLine, LogLevel.Information)
    .AddInterceptors(new InterceptadorDeComandos())
    .AddInterceptors(new InterceptadorDeConexao())
    ;



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
```



## 6 - Interceptando alterações.
```c#

optionsBuilder
    .UseSqlServer(stringDeConexao)
    .EnableSensitiveDataLogging()
    .LogTo(Console.WriteLine, LogLevel.Information)
    .AddInterceptors(new InterceptadorDeComandos())
    .AddInterceptors(new InterceptadorDeConexao())
    .AddInterceptors(new InterceptadorDePercistencia())
    ;


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

```


