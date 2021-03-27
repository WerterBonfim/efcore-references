# 03 - Database

* Ensure Created/Deleted
* Resolver GAP do Ensure Created
* HealthCheck do Banco de dados
* Gerenciar estado da conexão
* Commandos ExecuteSql
* Como se proteger de ataques Sql Injection
* Detectando migraçoes pendentes
* Forçando uma migração
* Recuperando migrações existentes em sua aplicação
* Recuperando migrações aplicadas em seu banco de dados
* Gerar Script Sql de seu modelo de dados

## 1 - Ensure Created/Deleted

Alternativa as migrações no ambiente de dev, para descartar e recriar 
o banco de dados

```c#

using var db = new ApplicationContext();
db.Database.EnsureCreated();
db.Database.EnsureDeleted();

```

## 2 - Resolvendo o GAP do Ensure Created



```c#

using var db1 = new ApplicationContext();
using var db2 = new ApplicationContextCidade();

db1.Database.EnsureCreated();
db2.Database.EnsureCreated();

// força a criação das tabelas do contexto 
var databaseCreator = db2.GetService<IRelationalDatabaseCreator>();
databaseCreator.CreateTables();

```

## 3 - HealthCheck do Banco de dados

```c#

using var db = new ApplicationContext();
var possoConectar = db.Database.CanConnect();

if (possoConectar) 
    Console.WriteLine("Banco online");    
else 
    Console.WriteLine("Banco não esta disponível");

```

## 4 - Gerenciar estado da conexão

```c#
using var db = new ApplicationContext();
var time = Stopwatch.StartNew();

var conexao = db.Database.GetDbConnection();

conexao.StateChange += (_, __) => ++_count;
if (gerenciarEstadoConexao) // 
    conexao.Open();

for (var i = 0; i < 800; i++)
    db.Departamentos.AsNoTracking().Any();

time.Stop();
var mensagem = $"Tempo: {time.Elapsed.ToString()}, {gerenciarEstadoConexao}, Contador: {_count}";
Console.WriteLine(mensagem);

```

```bash
Resultado 

Pooling false
Tempo: 00:00:02.5281803, gerenciar: False, Contador: 1600
Tempo: 00:00:00.1146067, gerenciar: True, Contador: 1


Pooling true
Tempo: 00:00:00.1659154, gerenciar: False, Contador: 1600
Tempo: 00:00:00.1292139, gerenciar: True, Contador: 1
```

## 5 - Commandos ExecuteSql

```c#

 using var db = new ApplicationContext();
            
//Primeira opção
using (var cmd = db.Database.GetDbConnection().CreateCommand())
{
    cmd.CommandText = "SELECT 1";
    cmd.ExecuteNonQuery();
}

// segunda opção
var descricao = "TTEste";
db.Database.ExecuteSqlRaw("update departamentos set descricao={0} where id = 1", descricao);

// terceira opção
db.Database.ExecuteSqlInterpolated($"update departamentos set descricao={descricao} where id = 1");

```

## 6 - Como se proteger de ataques Sql Injection

```c#

var injection = "Teste ' or 1='1";

var query = "update departamentos set descricao = 'AtaqueSqlInjection' " + 
            // não concatene string com parametros
            $"where descricao = '{injection}'";

db.Database.ExecuteSqlRaw(query);
foreach (var departamento in db.Departamentos.AsNoTracking())
{
    Console.WriteLine($"Id: {departamento.Id}, Descrição: {departamento.Descricao}");
}


```



## 7 - Detectando migraçoes pendentes

```bash
# adicionar uma migração, neste caso a primeira migração
dotnet ef migrations add Initial --context ApplicationContext
```
```c#
using var db = new ApplicationContext();

var migracoesPendentes = db.Database.GetPendingMigrations();
Console.WriteLine($"Total: {migracoesPendentes.Count()}");

foreach (var migracoes in migracoesPendentes)
    Console.WriteLine($"Migração: {migracoes}");


```


## 8 - Forçando uma migração

```bash
# Deletar a base de dados via console
dotnet ef database drop --context ApplicationContext
```

```c#
// Forçando a migração
using var db = new ApplicationContext();
db.Database.Migrate();
```



## 9 - Recuperando migrações existentes em sua aplicação

```bash
dotnet ef migrations add rg --context ApplicationContext
```

```C#
using var db = new ApplicationContext();
var migracoes = db.Database.GetMigrations();

Console.WriteLine($"Total: {migracoes.Count()}");

foreach (var migracao in migracoes)
    Console.WriteLine($"Migração: {migracao}");
```





## 10 - Recuperando migrações aplicadas em seu banco de dados
```bash
# lista as migrações e seu status
dotnet ef migrations list --context ApplicationContext
# output: 
#   20210326182519_Initial
#   20210327000259_rg (Pending)

# applicando as migrações pendentes via terminal
dotnet ef database update --context ApplicationContext

```

```
```

## 11 - Gerar Script Sql de seu modelo de dados
```c#
using var db = new ApplicationContext();
var script = db.Database.GenerateCreateScript();

Console.WriteLine(script);
```
