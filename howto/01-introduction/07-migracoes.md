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
 
# commit changes
dotnet ef database update -p ./Curso/CursoEFCore.csproj

# rollback
dotnet ef database update \ 
    # Name of migrations target
    PrimeiraMigracao \ 
    -p ./Curso/CursoEFCore.csproj

# Output
# Reverting migration '20210317235306_AdicionarEmail'.

# removing migratons
dotnet ef migrations remove -p ./Curso/CursoEFCore.csproj

```

command migrations remove deletes the migrations not committed in the database


# 9 - Pending Migrations

```c#
// To verify if has:
var exist = db.Database.GetPendingMigrations().Any();

```
