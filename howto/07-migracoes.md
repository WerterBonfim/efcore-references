### 1 - Dependencies

Microsoft.EntityFrameworkCore.Design

Microsoft.EntityFrameworkCore.Tools -> To use in console

To install globaly

```bash
dotnet tool install --global dotnet-ef --version 5.0.4
```

run first migraton

```bash
dotnet ef migrations add PrimeiraMigracao \ 
    # project
	-p ./Curso/CursoEFCore.csproj \ 
	# default is Migrations 
	-o PastaDeMigracoes \ 
	# For define context
	-c ApplicationContext 
	
```

# 6 - Applaying Migrations

Has tree ways to apply migrations:

* Script
* CLI
* Code

### By Script:

For generate script sql:

```bash
dotnet ef migrations script \
	# the path of projeto 
	-p ./Curso/CursoEFCore.csproj \
	# Generate script sql in the specified path
	-o ./Curso/PrimeiraMigracao.sql
	
```

### By CLI:


To add pending migrations:
```bash
dotnet ef database update \ 
    # project
    -p ./Curso/CursoEFCore.csproj \ 
    # verbose. Write output all steps
    -v
```

### By Code:
 

```C#

// add using Microsoft.EntityFrameworkCore;

using var db = new Data.ApplicationContext();
db.Database.Migrate();

```


# 7 - Generate Scripts Sql Idempotent

generate script with validations:
 * transaction
 * verify if objects exists
  



```bash

dotnet ef migrations script \ 
    # Project
    -p ./Curso/CursoEFCore.csproj \ 
    # Output path sql
    -o ./Curso/Idempotent.sql \ 
    # our -i
    --idempotent

```

# 8 - Rollback Migrations

```bash

dotnet ef migrations add AdicionarEmail -p ./Curso/CursoEFCore.csproj

```
