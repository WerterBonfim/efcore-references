## Setup

```bash

# SqlServer
Microsoft.EntityFrameworkCore.SqlServer

# Para as migrações via console 
Microsoft.EntityFrameworkCore.Design

# Para as migrações via console 
Microsoft.EntityFrameworkCore.Tools

# Para logar as querys no console 
Microsoft.Extensions.Logging.Console --version 5.0.0

```


```c#

protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    var stringDeConexao = "Server=localhost,1433;Database=DominandoEFCore;"; 
    
    optionsBuilder
        .UseSqlServer(stringDeConexao)
        .EnableSensitiveDataLogging()
        .LogTo(Console.WriteLine, LogLevel.Information);
}

```
