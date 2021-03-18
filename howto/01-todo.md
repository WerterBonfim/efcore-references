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
