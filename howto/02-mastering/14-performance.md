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
você pode ter uma degradação de performance.

## 2 - Resolução de identidade

```c#
```

## 3 - Desabilitando rastreamento de consultas

```c#
```

## 4 - Consulta com tipo anônimo rastreada

```c#
```

## 5 - Consultas projetadas

```c#
```
