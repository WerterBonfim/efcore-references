# Session 8 - Bonus

* Deletando propriedades não configuradas
* Resiliência da conexão
* Alterando o nome da tabela de histórico de migrações


## 1 - Deletando propriedades não configuradas

```c#
private void MapearPropriedadesEsquecidas(ModelBuilder modelBuilder) {
    foreach (var entity in modelBuilder.Model.GetEntityTypes())
    {
        var properties = entity.GetProperties()
            .Where(x => x.ClrType == typeof(string));

        foreach (var property in properties)
        {
            var maxLenghtIsNotDefined = 
                string.IsNullOrEmpty(property.GetColumnType()) &&
                !property.GetMaxLength().HasValue;

            if (maxLenghtIsNotDefined){

                //property.SetMaxLength(100);
                property.SetColumnType("VARCHAR(100)");

            }
        }
    }
}
```


## 2 - Resiliência da conexão

```c#

.UseSqlServer("Server=localhost,1433;Database=CursoEFCore;", 
        p => p.EnableRetryOnFailure(
            maxRetryCount: 2, // qtd de tentativa
            maxRetryDelay: TimeSpan.FromSeconds(5), // Aguardar 5 segundos após o erro
            errorNumbersToAdd: null
        ));


```

## 3 - Alterando o nome da tabela de histórico de migrações


```c#
.UseSqlServer("Server=localhost,1433;Database=CursoEFCore;",
                    p => p
                        .EnableRetryOnFailure(
                            maxRetryCount: 2, // qtd de tentativa
                            maxRetryDelay: TimeSpan.FromSeconds(5), // Aguardar 5 segundos após o erro
                            errorNumbersToAdd: null)
                        .MigrationsHistoryTable("curso_ef_core")
                );
```
