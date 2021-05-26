## Infraestrutura

* 1 - Configurando log simplificado
* 2 - Filtrado categoria de log
* 3 - Escrevendo log em um arquivo
* 4 - Habilitando erros detalhados
* 5 - Habilitando visualização dos dados sensíveis
* 6 - Configurando o batch size
* 7 - Configurando o timeout do comando global
* 8 - Configurando o timeout para um comando para um fluxo
* 9 - Confgurando resiliência para sua aplicação
* 10 - Criando uma estratégia de resiliência



### 1 - Configurando log simplificado
```c#
optionsBuilder    
    .UseSqlServer(stringDeConexao)    
    // Gerar um log com tudo o que esta contecendo
    //.LogTo(Console.WriteLine)
    // Gera os log somente informativos
    .LogTo(Console.WriteLine, LogLevel.Information)
    ;
```


### 2 - Filtrado categoria de log

Namespace responsavel: Microsoft.EntityFrameworkCore.Diagnostics

```c#
optionsBuilder
    .UseSqlServer(stringDeConexao)                
    .LogTo(Console.WriteLine,
        // Filtro de eventos
        new[] {CoreEventId.ContextInitialized, RelationalEventId.CommandExecuted},
        // log level
        LogLevel.Information,
        // options -> Data hora e em uma unica linha
        DbContextLoggerOptions.LocalTime | DbContextLoggerOptions.SingleLine
    )
    ;
```

### 3 - Escrevendo log em um arquivo
```c#
private readonly StreamWriter _writer = new StreamWriter("log_ef.txt", true);
optionsBuilder
    .UseSqlServer(stringDeConexao)
    .EnableSensitiveDataLogging()
    .LogTo(_writer.WriteLine)
    ;

public override void Dispose()
{
    base.Dispose();
    _writer.Dispose();
}
```


### 4 - Habilitando erros detalhados

Mudei o tipo da propriedade Departamento.Ativo de booleando para int.

Erro gerado:
```bash
# Unhandled exception. System.InvalidCastException: Unable to cast object of type 'System.Boolean' to type 'System.Int32'.

```

Para habilitar erros mais detalhados:
Use somente em ambiente de dev

```c#
optionsBuilder
    .UseSqlServer(stringDeConexao)
    // Gera um erro mais detalhado facilitando a manutenção dos erros
    .EnableDetailedErrors()
    
    ;

```

Output do erro depois de habilitar o erro detalhado:

```bash
# Unhandled exception. System.InvalidOperationException: An error occurred while reading a database value for property 'Departamento.Ativo'. The expected type was 'System.Int32' but the actual value was of type 'System.Boolean'.

#  ---> System.InvalidCastException: Unable to cast object of type 'System.Boolean' to type 'System.Int32'.

```





### 5 - Habilitando visualização dos dados sensíveis

Usar somente em ambiente de dev

```c#
 optionsBuilder
    .UseSqlServer(stringDeConexao)
    .EnableSensitiveDataLogging();
```


### 6 - Configurando o batch size

Tamanho limite padrão é 42

```c#
optionsBuilder
    .UseSqlServer(stringDeConexao, x => x.MaxBatchSize(50))
    .EnableSensitiveDataLogging()
    .LogTo(Console.WriteLine, LogLevel.Information)
    ;
```


### 7 - Configurando o timeout do comando global
```c#
 optionsBuilder
    .UseSqlServer(stringDeConexao, x => x
        .MaxBatchSize(50)
        .CommandTimeout(5) // 5 segundos
    )
    .EnableSensitiveDataLogging()
    .LogTo(Console.WriteLine, LogLevel.Information);


// forçando um erro
db.Database.ExecuteSqlRaw("WAITFOR DELAY '00:00:07'  ;SELECT 1");
```


### 8 - Configurando o timeout para um comando para um fluxo
```c#

db.Database.SetCommandTimeout(10);
db.Database.ExecuteSqlRaw("WAITFOR DELAY '00:00:07'  ;SELECT 1");
```

### 9 - Confgurando resiliência para sua aplicação
```c#
 optionsBuilder
    .UseSqlServer(stringDeConexao, x => x
        .MaxBatchSize(50)
        .CommandTimeout(5)
        .EnableRetryOnFailure(4, TimeSpan.FromSeconds(10), null)
    )
```

### 10 - Criando uma estratégia de resiliência

Cenário: Ocorrera um erro de rede logo após efetuar o SaveChanges. Como esta configurado o RetryOnFailure, o EF tentara executar novamente o SaveChange e com isso poderá salvar dados duplicados. O código abaixo é uma estrategia para evitar isso.


```c#
using var db = new ApplicationContext();

var strategy = db.Database.CreateExecutionStrategy();
strategy.Execute(() =>
{
    using var transaction = db.Database.BeginTransaction();

    db.Departamentos.Add(new Departamento {Descricao = "Departament transação"});
    db.SaveChanges();
    
    transaction.Commit();
});
```
