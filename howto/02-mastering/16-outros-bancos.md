# Acessando outros banco de dados

* 1 - PostgreSQL
* 2 - SQLite
* 3 - InMemory
* 4 - Azure Cosmos DB


## 1 - PostgreSQL

Nuget:
Npgsql.EntityFrameworkCore.PostgreSQL

```bash
docker run --rm --name postgres-efcore -p 5423:5432 -e POSTGRES_PASSWORD=123 -d postgres
```

```c#
const string strConnectionPg = "Host=localhost;Database=DominandoEFCore;Username=postgres;Password=123";

optionsBuilder
    .UseNpgsql(strConnectionPg)    
    .LogTo(Console.WriteLine, LogLevel.Information)
    .EnableSensitiveDataLogging();
```


## 2 - SQLite

Nuget:
Microsoft.EntityFrameworkCore.Sqlite

```c#

optionsBuilder    
    .UseSqlite("DataSource=DominandoEFCore.db")
    .LogTo(Console.WriteLine, LogLevel.Information)
    .EnableSensitiveDataLogging();

```


## 3 - InMemory

Nuget: Microsoft.EntityFrameworkCore.InMemory

```c#
optionsBuilder    
    .UseInMemoryDatabase(databaseName: "DominandoEFCore")
    .LogTo(Console.WriteLine, LogLevel.Information)
    .EnableSensitiveDataLogging();
```

## 4 - Azure Cosmos DB

Nuget: Microsoft.EntityFrameworkCore.Cosmos

```c#
optionsBuilder    
    .UseCosmos(
        accountEndpoint: "https://localhost:8081",
        accountKey: "",
        databaseName: "DominandoEFCore"
    )
    .LogTo(Console.WriteLine, LogLevel.Information)
    .EnableSensitiveDataLogging();

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Pessoa>(c =>
    {
        c.HasKey(x => x.Id);
        c.Property(x => x.Nome)
            .HasMaxLength(60)
            .IsUnicode(false);

        c.ToContainer("Pessoas");
    });
}
```
