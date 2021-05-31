# Dicas e truques

* 1 - Conhecer o método ToQueryString
* 2 - Depuração com DebugView
* 3 - Redefinir o estado do seu contexto
* 4 - Include com consulta filtrada
* 5 - SingleOrDefault vs FirstOrDefault
* 6 - Tabela sem chave primária
* 7 - Usando Views de seu banco de dados
* 8 - Forçar o uso do VARCHAR
* 9 - Aplicar conversão de nomeclatura
* 10 - Operadores de agregação
* 11 - Operadores de agregação no agrupamento
* 12 - Contadores de eventos


## 1 - Conhecer o método ToQueryString

```c#
using var db = new ApplicationContext();

var query = db.Departamentos.Where(x => x.Id == 2);

var sql = query.ToQueryString();

Console.WriteLine(sql);

```


## 2 - Depuração com DebugView

```
```


## 3 - Redefinir o estado do seu contexto

Ideal para ambientes de teste

```c#
db.ChangeTracker.Clear();
```


## 4 - Include com consulta filtrada

```c#
using var db = new ApplicationContext();

var sql = db
    .Departamentos
    .Include(p => p.Colaboradores.Where(x => x.Nome.StartsWith("Werter")))
    .ToQueryString();

Console.WriteLine(sql);

```

Output:

```sql
SELECT [d].[Id], [d].[Descricao], [t].[Id], [t].[DepartamentoId], [t].[Nome]
FROM [Departamentos] AS [d]
LEFT JOIN (
    SELECT [c].[Id], [c].[DepartamentoId], [c].[Nome]
    FROM [Colaboradores] AS [c]
    WHERE [c].[Nome] IS NOT NULL AND ([c].[Nome] LIKE N'Werter%')
) AS [t] ON [d].[Id] = [t].[DepartamentoId]
ORDER BY [d].[Id], [t].[Id]

```


## 5 - SingleOrDefault vs FirstOrDefault

SingleOrDefault: Quando existe mais de um elemento no resultado da consulta, gera uma exceção
FirstOrDefault: Retorna null caso não encontre um elemento

### SingleOrDefault

* Quando você precisar de 0 ou 1 elemento da consulta.
* Protege de informações duplicadas. EX: Não pode retornar um CPF duplicado
* Internamente o EFCore gera um SQL com TOP(2)

### FirstOuDefault
* Quando você precisar apenas de 1 elemento da consulta.
* Internamente o EFCore gera um SQL com TOP(1)


## 6 - Tabela sem chave primária

O EFCore não faz persistência de entidades sem chave primária

```c#

[Keyless]
public class UsuarioFuncao
{
    public Guid UsuarioId { get; set; }
    public Guid FuncaoId { get; set; }
}

// Ou

protected override void OnModelCreating(ModelBuilder modelBuilder)
{

    modelBuilder.Entity<UsuarioFuncao>()
        .HasNoKey();
    
    
}
```


## 7 - Usando Views de seu banco de dados

```c#

db.Database.ExecuteSqlRaw(
        @"CREATE VIEW vw_departamento_relatorio AS
        SELECT
            d.Descricao, COUNT(c.Id) AS Colaboradores
        FROM Departamentos AS d
    LEFT JOIN Colaboradores AS c ON c.DepartamentoId = d.Id
    GROUP BY d.Descricao");

protected override void OnModelCreating(ModelBuilder modelBuilder)
{

    modelBuilder.Entity<DepartamentoRelatorio>(e =>
    {
        e.HasNoKey();

        e.ToView("vw_departamento_relatorio");

        e.Property(x => x.Departamento)
            .HasColumnName("Descricao");
    });

    
}
```


## 8 - Forçar o uso do VARCHAR

```c#
var camposNVarchar = modelBuilder.Model.GetEntityTypes()
        .SelectMany(x => x.GetProperties())
        .Where(x => x.ClrType == typeof(string) && x.GetColumnType() == null);

foreach (var property in camposNVarchar)
    property.SetIsUnicode(false);
```


## 9 - Aplicar conversão de nomeclatura

```c#
public static class SnakeCaseExtensions
{
    public static void ToSnakeCaseNames(this ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName().ToSnakeCase();
            entity.SetTableName(tableName);

            foreach (var property in entity.GetProperties())
            {
                var storeObjectIdentifier = StoreObjectIdentifier.Table(tableName, null);

                var columnName = property.GetColumnName(storeObjectIdentifier).ToSnakeCase();
                property.SetColumnName(columnName);
            }

            foreach (var key in entity.GetKeys())
            {
                var keyName = key.GetName().ToSnakeCase();
                key.SetName(keyName);
            }

            foreach (var key in entity.GetForeignKeys())
            {
                var foreignKeyName = key.GetConstraintName().ToSnakeCase();
                key.SetConstraintName(foreignKeyName);
            }

            foreach (var index in entity.GetIndexes())
            {
                var indexName = index.GetDatabaseName().ToSnakeCase();
                index.SetDatabaseName(indexName);
            }
        }
    }

    private static string ToSnakeCase(this string name) =>
        Regex.Replace(name, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();

}



    modelBuilder.ToSnakeCaseNames();

```


## 10 - Operadores de agregação

```c#
using var db = new ApplicationContext();

var sql = db.Departamentos
    .GroupBy(x => x.Descricao)
    .Select(x => new
    {
        Descricao = x.Key,
        Contador = x.Count(),
        Media = x.Average(y => y.Id),
        Maximo = x.Max(y => y.Id),
        Soma = x.Sum(y => y.Id)
    }).ToQueryString();

Console.WriteLine(sql);

```

```sql
SELECT [d].[descricao] AS [Descricao],
       COUNT(*) AS [Contador],
       AVG(CAST([d].[id] AS float)) AS [Media],
       MAX([d].[id]) AS [Maximo],
       COALESCE(SUM([d].[id]), 0) AS [Soma]
FROM [departamentos] AS [d]
GROUP BY [d].[descricao]

```

## 11 - Operadores de agregação no agrupamento

```c#
using var db = new ApplicationContext();

var sql = db.Departamentos
    .GroupBy(x => x.Descricao)
    .Where(x => x.Count() > 1)
    .Select(x => new
    {
        Descricao = x.Key,
        Contador = x.Count(),
        Media = x.Average(y => y.Id),
        Maximo = x.Max(y => y.Id),
        Soma = x.Sum(y => y.Id)
    }).ToQueryString();

Console.WriteLine(sql);

```

```sql
SELECT [d].[descricao]              AS [Descricao],
       COUNT(*)                     AS [Contador],
       AVG(CAST([d].[id] AS float)) AS [Media],
       MAX([d].[id])                AS [Maximo],
       COALESCE(SUM([d].[id]), 0)   AS [Soma]
FROM [departamentos] AS [d]
GROUP BY [d].[descricao]
HAVING COUNT(*) > 1
```


## 12 - Contadores de eventos

```
```


