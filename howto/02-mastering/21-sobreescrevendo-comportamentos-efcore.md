# Sobreescrevendo comportamentos do EF Core

```c#

optionsBuilder
    .LogTo(Console.WriteLine)
    .UseSqlServer("Server=localhost, 1433;Database=DominandoEFCore;User Id=sa;Password=!123Senha;")
    .EnableSensitiveDataLogging()
    .ReplaceService<IQuerySqlGeneratorFactory, MySqlServerQuerySqlGeneratorFactory>();


public class MySqlServerQueryGenerator : SqlServerQuerySqlGenerator
{
    public MySqlServerQueryGenerator(QuerySqlGeneratorDependencies dependencies) : base(dependencies)
    {
        
    }

    protected override Expression VisitTable(TableExpression tableExpression)
    {
        var table = base.VisitTable(tableExpression);
        Sql.Append(" WITH (NOLOCK)");
        return table;
    }
}

public class MySqlServerQuerySqlGeneratorFactory : SqlServerQuerySqlGeneratorFactory
{
    private readonly QuerySqlGeneratorDependencies _dependencies;
    
    public MySqlServerQuerySqlGeneratorFactory(QuerySqlGeneratorDependencies dependencies) : base(dependencies)
    {
        _dependencies = dependencies;
    }

    public override QuerySqlGenerator Create()
    {
        return new MySqlServerQueryGenerator(_dependencies);
    }
}
```
