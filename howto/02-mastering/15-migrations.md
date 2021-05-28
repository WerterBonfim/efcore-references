# Migrations

* 1 - Migrações do EF Core
* 2 - O que é necessário para criar uma migração?
* 3 - Gerando uma migração
* 4 - Analisando arquivos da migração
* 5 - Gerando script SQL
* 6 - Gerando script SQL idempotente
* 7 - Aplicando migração no banco de dados
* 8 - Desfazendo uma migração
* 9 - Migrações pendentes
* 10 - Engenharia Reversa


## 1 - Migrações do EF Core

Faz o versionamento do seu banco de dados

## 2 - O que é necessário para criar uma migração?

Microsoft.EntityFrameworkCore.Desing -> Responsavel por gerar as migrações
Microsoft.EntityFrameworkCore.Tools -> Superset de comandos para executar no console



```bash
# instalar globalmente
dotnet tool install --global dotnet-ef --version 5.0.4
```

## 3 - Gerando uma migração

```c#
public class ApplicationContext : DbContext
{
    private string _stringDeConexao => new StringBuilder()
        .Append("Server=localhost, 1433;")
        .Append("Database=TreinoMigrations;")
        .Append("User Id=sa;")
        .Append("Password=!123Senha;")
        .Append("Application Name=\"Rider CursoEFCore\";")
        .Append("pooling=true;")
        .ToString();
    
    public DbSet<Pessoa> Pessoas { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlServer(_stringDeConexao)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pessoa>(conf =>
        {
            conf.HasKey(x => x.Id);
            conf.Property(p => p.Nome)
                .HasMaxLength(60)
                .IsUnicode(false);
        });
    }
}
```

```bash
dotnet ef migrations add PrimeiraMigracao -p ./DominandoEFCore.AulaMigrations.csproj
```

## 4 - Analisando arquivos da migração

```bash
# Caso queira mudar o diretório onde é gerado as migrações
doent ef migratons add .\src\EFCore.csproj -o NomeDiretorio
```

São gerado 2 arquivos (linux, EFCore versão 5.0.3) 

20210528111101_PrimeiraMigracao.cs -> Contem todas as alterações que iram acontecer com a 
sua base de dados
ApplicationContextModelSnapshot.cs -> Foto do estado atual do modelo de dados, é atualizado
a cada migration

## 5 - Gerando script SQL

```bash
dotnet ef migrations script -p .\src\EFCore.csproj -o .\src\MeuScript.sql
```

## 6 - Gerando script SQL idempotente

```c#
```

## 7 - Aplicando migração no banco de dados

```c#
```

## 8 - Desfazendo uma migração

```c#
```

## 9 - Migrações pendentes

```c#
```

## 10 - Engenharia Reversa

```c#
```

