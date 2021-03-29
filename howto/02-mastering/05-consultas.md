## Consultas 

* 1 - Configurando um filtro global
* 2 - Ignorando filtros globais
* 3 - Consultas projetadas
* 4 - Consulta parametrizada
* 5 - Consulta interpolada
* 6 - Usando o recurso TAG em consultas
* 7 - Entendendo diferença em consulta 1xN vs Nx1
* 8 - Divisão de consultas com SplitQuery



### 1 - Configurando um filtro global

Introduzido na versão 2.0


```c#

// No DbContext. Agora toda consulta efetuada na tabela Departamento, incluira esse filtro
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Departamento>().HasQueryFilter(x => x!.Excluido);
}
```



### 2 - Ignorando filtros globais
```c#
var departamentos = db.Departamentos
    .IgnoreQueryFilters()
    .Where(p => p.Id > 0)
    .ToList();
```


### 3 - Consultas projetadas

Consultas projetadas são uma boa prática para diminir a quantidade de 
dados materializadas na base de dados. Deixando o retorno mais enxuta.

```c#
var departamentos = db.Departamentos
    .Where(p => p.Id > 0)
    .Select(p => new
    {
        p.Descricao,
        Funcionarios = p.Funcionarios
            .Select(f => f.Nome)
    })
    .ToList();
```



### 4 - Consulta parametrizada
```C#

var id = 0;

var departamentos = db.Departamentos
    .FromSqlRaw("SELECT * FROM Departamentos WITH(NOLOCK) WHERE id > {0}", id)
    .Where(p => !p.Excluido)
    .ToList();
```

### 5 - Consulta interpolada
```C#

var id = 0;

var departamentos = db.Departamentos
    .FromSqlInterpolated($"SELECT * FROM Departamentos WITH(NOLOCK) WHERE id > {id}" )
    .Where(p => !p.Excluido)
    .ToList();
```

### 6 - Usando o recurso TAG em consultas
```c#
var departamentos = db.Departamentos
    .TagWith(@"Estou enviando um comentário para o servidor

            segundo comentário qualquer
            terceiro comentário ")
    .ToList();
```

### 7 - Entendendo diferença em consulta 1xN vs Nx1
```c#
var departamentos = db.Departamentos
    .Include(p => p.Funcionarios)
    .ToList();
```

```bash

# retorno: LEFT JOIN
# SELECT [d].[Id], [d].[Ativo], [d].[Descricao], [d].[Excluido], [f].[Id], [f].[Cpf], 
# [f].[DepartamentoId], [f].[Excluido], [f].[Nome], [f].[Rg]
# FROM [Departamentos] AS [d]
# LEFT JOIN [Funcionarios] AS [f] ON [d].[Id] = [f].[DepartamentoId]
# ORDER BY [d].[Id], [f].[Id]

```


```c#
var funcionarios = db.Funcionarios
    .Include(p => p.Departamento)
    .ToList();
```

```bash
# retorno: INNER JOIN
# SELECT [f].[Id], [f].[Cpf], [f].[DepartamentoId], [f].[Excluido], [f].[Nome], [f].[Rg], 
# [d].[Id], [d].[Ativo], [d].[Descricao], [d].[Excluido]
# FROM [Funcionarios] AS [f]
# INNER JOIN [Departamentos] AS [d] ON [f].[DepartamentoId] = [d].[Id]

```

### 8 - Divisão de consultas com SplitQuery

A divição de consultas basicamente veio para solucionar um problema, e basicamente o problema é quando precisamos carregar dados relacionados.

```c#
var departamentos = db.Departamentos
    .Include(p => p.Funcionarios)
    .Where(p => p.Id < 3)
    .ToList();
```

```bash
#  SELECT [d].[Id], [d].[Ativo], [d].[Descricao], [d].[Excluido], [f].[Id], [f].[Cpf], [f].[DepartamentoId], 
#  [f].[Excluido], [f].[Nome], [f].[Rg]
#  FROM [Departamentos] AS [d]
#  LEFT JOIN [Funcionarios] AS [f] ON [d].[Id] = [f].[DepartamentoId]
#  WHERE [d].[Id] < 3
#  ORDER BY [d].[Id], [f].[Id]


```

```c#
var departamentos2 = db.Departamentos
    .Include(p => p.Funcionarios)
    .Where(p => p.Id < 3)
    .AsSplitQuery()
    .ToList();
```

```bash
#  SELECT [d].[Id], [d].[Ativo], [d].[Descricao], [d].[Excluido]
#  FROM [Departamentos] AS [d]
#  WHERE [d].[Id] < 3
#  ORDER BY [d].[Id]


#  SELECT [f].[Id], [f].[Cpf], [f].[DepartamentoId], [f].[Excluido], [f].[Nome], [f].[Rg], [d].[Id]
#  FROM [Departamentos] AS [d]
#  INNER JOIN [Funcionarios] AS [f] ON [d].[Id] = [f].[DepartamentoId]
#  WHERE [d].[Id] < 3
#  ORDER BY [d].[Id]
```


Para configurar o SplitQuery de forma global:

```c#

optionsBuilder
    .UseSqlServer(stringDeConexao, 
        // Define globalmente
        p => p.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
    .EnableSensitiveDataLogging()
    //.UseLazyLoadingProxies()
    .LogTo(Console.WriteLine, LogLevel.Information)



var departamentos2 = db.Departamentos
    .Include(p => p.Funcionarios)
    .Where(p => p.Id < 3)
    // Podemos omitir se for configurado globalmente
    //.AsSplitQuery()
    // Caso queira ignorar a configuração global, 
    // ira se comportar de forma padrão
    //.AsSingleQuery()
    .ToList();

```




