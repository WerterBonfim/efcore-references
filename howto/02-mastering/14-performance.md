# Performance

* 1 - Tracking vs NoTracking
* 2 - Resolução de identidade
* 3 - Desabilitando rastreamento de consultas
* 4 - Consulta com tipo anônimo rastreada
* 5 - Consultas projetadas

## 1 - Tracking vs NoTracking

### Anatomia de Tracking/Trastreadas

É a funcionalidade padrão do EF.

```c#
var cursos = db.Cursos.Where(x => x.Id < 10).ToList();
```

A instância de cada registro é marcada com Unchanged dentro do seu DbContext.

Com isso o EF é capaz de detectar as instâncias que foram manipuladas.

### Anatomia de NoTracking/Não rastreada


```c#
var cursos = db
    .Cursos
    .AsNoTracking()
    .Where(x => x.Id < 10)
    .ToList();
```

Simplesmente materializa cada entidade. 

Dependendo da quantidade de registros que você busca do banco de dados,
você pode ter uma degradação de desempenho.

## 2 - Resolução de identidade

Resultado da analise:

```C#
Rastreada
└── Departamentos 1 instância 40 bytes

Não Rastreada
└── Departamentos 100 instâncias 4000 bytes

Com resolução de identidade
└── Departamentos 1 instância 40 bytes
```

```c#

using var db = new ApplicationContext();
var funcionarios = db
    .Funcionarios
    .AsNoTrackingWithIdentityResolution()
    .Include(x => x.Departamento)
    .ToList();
```

## 3 - Desabilitando rastreamento de consultas

Por padrão, todas as consultas são rastreadas.

A três cenários de configuração

### 1 - Direto na consulta

```c#

    // Por default é AsTracking
    db.Funcionarios    
        // Redudante pois por padrão todas as consultas são rastreadas
        //.AsTracking()
        .Include(x => x.Departamento)
        .ToList();

    // Não rastrear
    db.Funcionarios    
        .AsNoTracking()
        .Include(x => x.Departamento)
        .ToList();

    // Resolução de identidade
    db.Funcionarios
        .AsNoTrackingWithIdentityResolution()    
        .Include(x => x.Departamento)
        .ToList();

```


### 2 - Configurando no DbContext

No DbContext é possível customizar o comportamento através do Enum: QueryTrackingBehavior.

```c#

optionsBuilder
    .UseSqlServer(stringDeConexao)
    .EnableSensitiveDataLogging()
    .LogTo(Console.WriteLine, LogLevel.Information)
    // Parametros: TrackAll (Default), NoTracking e NoTrackingWithIdentityResolution
    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution)
```


### 3 - Alterando o comportamento na consulta

Para habilitar em uma determinada consulta



```c#

private static void ConsultaCustomizada1()
{
    using var db = new ApplicationContext();

    // Foi definido do DbContext NoTrackingWithIdentityResolution
    // como comportamento padrão    
    
    var funcionarios = db
        .Funcionarios
        // Irei rastrear essa consulta
        .AsTracking()
        .Include(x => x.Departamento)
        .ToList();
}

private static void ConsultaCustomizada2()
{
    using var db = new ApplicationContext();


    // Em tempo de execução dessa instancia, irei rastrear todas as consultas,
    // na instância do seu contexto
    db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
    
    var funcionarios = db
        .Funcionarios
        .Include(x => x.Departamento)
        .ToList();
}
```

## 4 - Consulta com tipo anônimo rastreada

Consulta projetadas anonimas também são rastreadas pelo EF, com isso
você pode atualizar informações normalmente.

```c#
using var db = new ApplicationContext();

var departamentos = db.Departamentos
    .Include(x => x.Funcionarios)
    .Select(x => new
    {
        Departamento = x,
        TotalFuncionarios = x.Funcionarios.Count()
    })
    .ToList();

departamentos[0].Departamento.Descricao = "Departamento teste atualizado";

db.SaveChanges();
```

## 5 - Consultas projetadas

As consultas projetadas tem uma melhor performance dependendo do cenário

```c#
private static void ComProjecao()
{
    using var db = new ApplicationContext();

    var comProjecao = db.Departamentos
        .Select(x => x.Descricao)
        .ToArray();

    var memoriaComProjecao = (System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024) + " MB";
    Console.WriteLine($"Total de memoria utilizada: {memoriaComProjecao}");
}

private static void SemProjecao()
{
    using var db = new ApplicationContext();

    var semProjecao = db.Departamentos.ToArray();

    var memoriaSemProjecao = (System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024) + " MB";
    Console.WriteLine($"Total de memoria utilizada: {memoriaSemProjecao}");
    
}
```
