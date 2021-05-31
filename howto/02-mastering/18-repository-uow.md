# Padrão Repository & UoW

* 1 - O que é Repository Pattern?
* 2 - O que é Unit-of-work?
* 3 - DbContext já é um padrão Repository & Uow
* 4 - Preparando o ambiente
* 5 - Implementando UoW - Estratégia 1
* 6 - Implementando UoW - Estragédia 2
* 7 - Criando um Repositório Genérico
* 8 - Consulta com Repositório Genérico 


## 1 - O que é Repository Pattern?

Faz a mediação entre o domínio e as camadas de mapeamento de dados
usando uma interface semelhante a uma coleção para acessar objetos de domínio.
(Martin Fowler)

Basicamente é um objeto que faz o isolamento das entidade do domínio de seu código que faz acesso à dados.

Prós
* Um único ponto de acesso à dados.
* Encapsulamento da lógica de acesso à dados
* SPOF (Ponto único de falha)

Contras
* Maior complexidade

## 2 - O que é Unit-of-work?

Podemos definir Unidade de trabalho(Unit of Work) como uma única transação
que pode envolver múltiplas operações tais como: (inserção/atualização/exclusão).

Mantém uma lista de objetos de negócio afetados por uma transação e coordena a 
escrita a partir de alterações e a resolução de problemas de concorrência. (Martin Fowler)

## 3 - DbContext já é um padrão Repository & Uow

### Vantagens:

* Testabilidade, dados que a gente consegue criar testes "mockados" para testar
parte isolada de regras de negócio.

* Acesso misto ao acesso a dados.

### Desvantagens:
* Complexidade.


## 4 - Preparando o ambiente



## 5 - Implementando UoW - Estratégia 1

```c#
public interface IUnitOfWork : IDisposable
{
    bool Commit();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationContext _context;
    

    public UnitOfWork(ApplicationContext context)
    {
        _context = context;
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public bool Commit()
    {
        return _context.SaveChanges() > 0;
    }
}

public interface IDepartamentoRepository
{
    Task<Departamento> GetByIdAsync(int id);
    void Add(Departamento departamento);
    bool Save();
}


[HttpPost]
public IActionResult CriarDepartamento([FromBody]Departamento departamento)
{
    _departamento.Add(departamento);
    _departamento.Save();
    

    return Ok(departamento);
}
```




## 6 - Implementando UoW - Estragédia 2



```c#

public interface IUnitOfWork : IDisposable
{
    bool Commit();
    IDepartamentoRepository DepartamentoRepository { get; }
}

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationContext _context;
    

    public UnitOfWork(ApplicationContext context)
    {
        _context = context;
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public bool Commit()
    {
        return _context.SaveChanges() > 0;
    }

    // A desvantagem é se ouver varios repositórios
    private IDepartamentoRepository _departamentoRepository;

    public IDepartamentoRepository DepartamentoRepository => 
        _departamentoRepository ?? (_departamentoRepository = new DepartamentoRepository(_context));
}

[HttpGet("{id}")]
public async Task<IActionResult> GetByIdAsync(int id)
{
    var departamento = await _uow.DepartamentoRepository.GetByIdAsync(id);
    return Ok(departamento);
}

[HttpPost]
public IActionResult CriarDepartamento([FromBody]Departamento departamento)
{
    _uow.DepartamentoRepository.Add(departamento);
    
    var salvou = _uow.Commit();

    return Ok(departamento);
}
```



## 7 - Criando um Repositório Genérico

```c#

// Iguas ao exemplo anterior
public interface IUnitOfWork : IDisposable {}
public class UnitOfWork : IUnitOfWork {}

/// <summary>
/// Interface de marcação
/// </summary>
public interface IRepository {}

public interface IGenericRepository<T> where T : class, IRepository
{
    void Add(T entity);
    void Remove(T entity);
    void Update(T entity);

    Task<T> GetByIdAsync(int id);
    Task<T> FirstAsync(Expression<Func<T, bool>> expression);
    Task<int> CountAsync(Expression<Func<T, bool>> expression);

    Task<List<T>> GetDataAsync(
        Expression<Func<T, bool>> expression = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
        int skip = 0,
        int take = 10
    );
}

public class GenericRepository<T> : IGenericRepository<T> where T : class, IRepository
{
    private readonly DbSet<T> _dbSet;

    public GenericRepository(ApplicationContext context)
    {
        _dbSet = context.Set<T>();
    }
    
    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T> FirstAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.FirstAsync(expression);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.CountAsync(expression);
    }

    public async Task<List<T>> GetDataAsync(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int skip = 0, int take = 10)
    {
        var query = _dbSet.AsQueryable();

        if (expression != null) query = query.Where(expression);

        if (include != null) query = include(query);

        query = query.Take(take).Skip(skip);

        return await query.ToListAsync();
    }
}


public class DepartamentoRepository : GenericRepository<Departamento>, IDepartamentoRepository
{ 
    public DepartamentoRepository(ApplicationContext context) : base(context)
    {
        
    }
}



```



## 8 - Consulta com Repositório Genérico 
```c#

//api/departamentos?descricao=teste
[HttpGet]
public async Task<IActionResult> Consultar([FromQuery] string descricao)
{
    var departamentos = await _uow.DepartamentoRepository
        .GetDataAsync(
            x => x.Descricao.Contains(descricao),
            x => x.Include(y => y.Colaboradores)
            );

    return Ok(departamentos);

}
```



