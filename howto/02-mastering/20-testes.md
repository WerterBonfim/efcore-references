# Testes

* 1 - Criando testes usando o provider InMemory
* 2 - Criando testes usando o provider SQLite


## 1 - Criando testes usando o provider InMemory

Limitações: Não é possivel utilizar Function do SqlServer que estão mapeapdas no provider
do Sql Server

```c#


private ApplicationContext CreateContext()
{
    var options = new DbContextOptionsBuilder<ApplicationContext>()
        .UseInMemoryDatabase("InMemoryTeste")
        .Options;

    return new ApplicationContext(options);
}
```


## 2 - Criando testes usando o provider SQLite

Quando usamos o SQLite em memoria ele trabalha de forma transiente. Quando a conexão é
fechada, os dados são descartadas

```c#

[Theory]
[InlineData("Tecnologia")]
[InlineData("RH")]
public void DeveInserirEConsultarUmDepartamento(string descricao)
{
    // Arg
    var departamento = new Departamento
    {
        Descricao = descricao,
        DataCadastro = DateTime.Now
    };

    // Setup
    var context = CreateContext();
    context.Database.EnsureCreated();
    context.Departamentos.Add(departamento);
    
    // Act

    var inseridos = context.SaveChanges();
    var departamentoInserido = context.Departamentos.FirstOrDefault(x => x.Descricao == descricao);

    // Assert
    Assert.Equal(1, inseridos);
    Assert.Equal(departamento, departamentoInserido);
}


private ApplicationContext CreateContext()
{
    // SQLite em memoria
    var conexao = new SqliteConnection("DataSource=:memory:");
    conexao.Open();
    var options = new DbContextOptionsBuilder<ApplicationContext>()
        .UseSqlite(conexao)
        .Options;

    return new ApplicationContext(options);
}
```

