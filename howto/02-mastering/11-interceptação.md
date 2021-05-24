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


```c#

```

[doc-interceptacao]:https://docs.microsoft.com/en-us/ef/core/logging-events-diagnostics/interceptors

## 4 - Aplicando hint NOLOCK nas consultas
```
```


## 5 - Interceptando abertura de conexão com o banco
```
```


## 6 - Interceptando alterações.
```
```


