## Stored Procedure

* 1 - Criando uma procedure de inserção
* 2 - Executando uma inserção via procedure
* 3 - Criando uma procedure de consulta
* 4 - Executando uma consulta via procedure



### 1 - Criando uma procedure de inserção
```c#
var criarDepartamento = "script de criação de procedure";
db.Database.ExecuteSqlRaw(criarDepartamento);
```


### 2 - Executando uma inserção via procedure
```c#
//db.Database.ExecuteSqlRaw("execute CriarDepartamento @p0, @p1", new object[] {"", ""});
db.Database.ExecuteSqlRaw("execute CriarDepartamento @p0, @p1", 
    "Departamento Via Procedure", true);
```


### 3 - Criando uma procedure de consulta
```c#
var script = "script sql de consulta";
db.Database.ExecuteSqlRaw(script);
```

### 4 - Executando uma consulta via procedure
```c#

var departamentos = db.Departamentos
    // Por debaixo dos panos ele gerar um @p0
    //.FromSqlRaw("execute ListarDepartamentos {0}", "dep")
    //.FromSqlRaw("execute ListarDepartamentos @dep", dep)
    //.FromSqlInterpolated($"execute ListarDepartamentos {dep}")
    .FromSqlRaw("execute ListarDepartamentos @p0", "dep")
    .ToList();
```
