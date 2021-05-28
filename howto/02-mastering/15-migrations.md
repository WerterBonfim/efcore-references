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
Gera scripts com validação, verificando se o objeto existe no banco

```bash

dotnet ef migrations script \
    # Projeto
    -p ./DominandoEFCore.AulaMigrations.csproj  \
    # Onde sera gerado o script
    -o ./Scripts/PrimeiraMigracaoIdempotente.sql \
    # alias para o parametro --idempontent
    -i
```

## 7 - Aplicando migração no banco de dados

Uma boa pratica para aplicar as migrações no banco de dados é 
atraves da geração do script. Dessa forma podemos fornecer esse script
para o DBA.

Outra forma é através de um processo de DevOps onde o script é gerado via pipeline.

Outras formas (shell e código) são indicadas somente para o ambiente de desenvolvimento.

### Via shell

```bash
dotnet ef database update \
    # Projeto
    -p ./DominandoEFCore.AulaMigrations.csproj \
    # Context
    --context ApplicationContext

```

### Via codigo

```c#

db.Database.Migrate();

```


## 8 - Desfazendo uma migração

```bash
# Revertendo uma migração
dotnet ef database update NomeMigracaoAnterior -p ./DominandoEFCore.AulaMigrations.csproj 

# removendo migrações
dotnet ef database remove -p ./DominandoEFCore.AulaMigrations.csproj 
```

## 9 - Migrações pendentes 

```bash
# Lista as migrações pendentes
dotnet ef migrations list -p ./DominandoEFCore.csproj
```

```c#
// Verifica via código se existe migrações pendentes
var exist = db.Database.GetPendingMigrations().Any();


var migracoes = db.Database.GetPendingMigrations();
foreach(var migracao in migracoes) {
    Console.WriteLine(migracao)
}

```


## 10 - Engenharia Reversa

Para executar esse exemplo, a um arquivo teste-scaffold.sql na pasta scripts para exemplificar a engenharia reversa.

```bash
dotnet ef dbcontext scaffold \ 
    # String de conexão, Deve ter o mesmo do banco de dados
    'Server=localhost, 1433;Database=EFCoreScaffoldExemploContext;User Id=sa;Password=!123Senha;Application Name="Rider CursoEFCore";pooling=true;' \
    # Provider do banco de dados
    Microsoft.EntityFrameworkCore.SqlServer \
    # Passe todas as tabelas 
    --table Produtos \
    --table Usuarios \
    # Preservar todos os nomes. 
    --use-database-names
    # usa todos os data annotations possiveis
    --data-annotations
    # Diretório do contexto
    --context-dir ./Context
    # Diretório onde todo o modelo de dados sera criado
    --output-dir ./Entidades
    --namespace Meu.Namespace
    --context-namespace Meu.NameSpace.Contexto
    --project ./DominandoEFCore.AulaMigrations.csproj
```

## Resumo

```bash

# Adicionando um migração
dotnet ef database add PrimeiraMigracao -p ./DominandoEFCore.AulaMigrations.csproj --context ApplicationContext

# Revertendo uma migração
dotnet ef database update NomeMigracaoAnterior -p ./DominandoEFCore.AulaMigrations.csproj 

# removendo migrações
dotnet ef database remove -p ./DominandoEFCore.AulaMigrations.csproj 

# Lista as migrações pendentes
dotnet ef migrations list -p ./DominandoEFCore.csproj

# Gerar script
dotnet ef migrations script -p .\src\EFCore.csproj -o .\src\MeuScript.sql

# Gerar script idemponent
dotnet ef migrations script \
    # Projeto
    -p ./DominandoEFCore.AulaMigrations.csproj  \
    # Onde sera gerado o script
    -o ./Scripts/PrimeiraMigracaoIdempotente.sql \
    # alias para o parametro --idempontent
    -i

```
