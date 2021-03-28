# 04 Tipos de carregamentos - DeepDive

* 1 - Carregamento Adiantado (Eager)
* 2 - Carregamento Explícito
* 3 - Carregamento Lento (Famoso LazyLoad)

## 1 - Adiantado (Eager)

Departamento/Funcionarios

Departamento possui muitos Funcionarios

Um Funcionario possui um departamento.

Força o relacionamento trazendo os dados das propriedades de navegação.
E um otimo recurso para quando a poucos dados a ser retornado.

TradeOff: Se ouver muito dados pode haver uma explosão cartesiana. As linhas do lado unico sempre serão duplicadas para os registro correspondentes no lado de muitos. Para cada funcionario sera carregando novamente um departamento.

```c#
var departamentos = db
    .Departamentos
    .Include(p => p.Funcionarios);
```



## 2 - Carregamento Explícito

Significa que os dados relacionados são explicitamente carregado do banco de dados
em um momento posterior, ou seja, quando você solicitar os dados.

```c#
var departamentos = db
            // Se não for chamado o .ToList() 
            // a conexão fica aberta, pode gerar um erro.
            // Com ToList os dados são buscados do banco de dados
            // e o EF fecha a conexão.
            // Outra alternativa e utilizar a propriedade
            // MultipleActiveResultSets=true na string de conexão             
            .Departamentos 
            .ToList();


foreach (var departamento in departamentos)
{
    if (departamento.Id == 2)
    {
        db.Entry(departamento)
            //.Collection("Funcionarios")
            .Collection(p => p.Funcionarios)
            // .Load(); // Carrega todos os funcionarios do departamento
            .Query()
            // Efetua um filtro em cima dos funcionario do departamento.
            // Uma maneira de aplicar as regras de negocio 
            .Where(p => p.Id > 2)
            .ToList();
    }

    Console.WriteLine("---------------------------------------------------------");
    Console.WriteLine($"Departamento: {departamento.Descricao}");

    if (departamento.Funcionarios?.Any() ?? false)
        foreach (var funcionario in departamento.Funcionarios)
            Console.WriteLine($"\tFuncionario: {funcionario.Nome}");
    else
        Console.WriteLine($"\tNenhum funcionario encontrado!");
}
```

```
```

## 3 - Carregamento Lento (Famoso LazyLoad)


Significa que os dados relacionados são carregado sob demanda do banco de dados
quando a propriedade de navegação é acessada.

Dependencia: Microsoft.EntityFrameworkCore.Proxies --version 5.0.4

```bash
# Dependencia para utilizar o LazyLoad
dotnet add package Microsoft.EntityFrameworkCore.Proxies --version 5.0.4
```

Ha três maneiras de carregas os dados:
* 1 - Proxies
* 2 - ILazyLoader
* 3 - Action

### 1 - Proxies

O EFCore utiliza por debaixo dos panos o proxie Castle.Proxies


```c#

// adicionar o metodo de extenção UseLazyLoadingProxies() no DbContext
optionsBuilder
        .UseSqlServer(stringDeConexao)
        .EnableSensitiveDataLogging()
        .UseLazyLoadingProxies()
        .LogTo(Console.WriteLine, LogLevel.Information)
        ;

// Nas classe de navegação deve adicionar o modificador virtual para
// o EFCore sobrescrever o comportamento
public virtual List<Funcionario> Funcionarios { get; set; }
public virtual Departamento Departamento { get; set; }

```

Para desabilitar o LazyLoad em um lugar expecifico:

```c#
// Desablitar lazy dentro de uma consulta especificada
db.ChangeTracker.LazyLoadingEnabled = false;
```


### 2 - ILazyLoader


```c#

// Remover do DbContext a configuração .UseLazyLoadingProxies()
// Remover virtual das propriedades

private ILazyLoader _lazyLoader;

private List<Funcionario> _funcionarios;
public List<Funcionario> Funcionarios
{
    get => _lazyLoader.Load(this, ref _funcionarios);
    set => _funcionarios = value;
}



// EFCore
public Departamento()
{
    
}

private Departamento(ILazyLoader lazyLoader)
{
    _lazyLoader = lazyLoader;
}
```


### 3 - Action


```c#

private Action<object, string> _lazyLoader;
// O nome do parametro deve ser extritamente lazyLoader
private Departamento(Action<object, string> lazyLoader)
{
    _lazyLoader = lazyLoader;
}
// EFCore
public Departamento()
{
    
}


private List<Funcionario> _funcionarios;
public List<Funcionario> Funcionarios
{
    get
    {
        _lazyLoader?.Invoke(this, nameof(Funcionarios));
        return _funcionarios;
    }
    set => _funcionarios = value;

}

```

