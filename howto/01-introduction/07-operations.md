# Session 7 - Operations

* 1 - Insert
* 2 - Read
* 3 - Update
* 4 - Delete


## 1 - Insert

```c#
    var produto = new Produto {
        Descricao = "Produto teste",
        CodigoBarras = "12345512344",
        Valor = 10m,
        TipoProduto = TipoProduto.MercadoriaParaRevenda,
        Ativo = true
    };

    using var db = new Data.ApplicationContext();

    // There are four ways to insert an item

    // Recomended
    db.Produtos.Add(produto);
    // Recomended
    db.Set<Produto>().Add(produto);

    db.Entry(produto).State = EntityState.Added;
    db.Add(produto);

    var rowsAffeted = db.SaveChanges();

    // Only 1 register is inserted, because of changer traker
    // There is only one product instance

    Console.WriteLine($"Total Rows {rowsAffeted}");
```

## 2 - Bulk Insert

```bash
# Logging
dotnet add package Microsoft.Extensions.Logging.Console --version 5.0.0

```


```c#
// add new private field
 private static readonly ILoggerFactory _logger =
            LoggerFactory.Create(p => p.AddConsole());

 protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
 {
     optionsBuilder
         .UseLoggerFactory(_logger)
         .EnableSensitiveDataLogging()
         .UseSqlServer("Server=localhost,1433;Database=CursoEFCore;User Id=sa; Password=!123Senha;");
 }



using var db = new Data.ApplicationContext();
db.AddRange(product, client, anotherEntity);
 
    
```

## 3 - Read
* Linq
* Extensions


```c#
using var db = new Data.ApplicationContext();
//var consultaPorSintaxe = (from c in db.Clientes where c.Id > 0 select c).ToList();
//var consultaPorMetodo = db.Clientes.Where(p => p.Id > 0).ToList();
var consultaPorMetodo = db.Clientes
    //.AsNoTracking()
    .Where(p => p.Id > 0)
    .OrderBy(x => x.Id)
    .ToList();


foreach (var cliente in consultaPorMetodo)
{
    System.Console.WriteLine($"Consultado o cliente {cliente.Id}");
    //var teste = db.Clientes.Find(cliente.Id); // check changerTracker
    // access database 
    var teste = db.Clientes.FirstOrDefault(x => x.Id == cliente.Id);
    System.Console.WriteLine(teste.ToString());
}
```


Only the `find` method tries to search the memory if there is data, the other methods perform a search in the database



## 4 - Lazy load

* Carregamento Adiantado
* Carregamento explícito
* Carregamento lento


### Carregamento Adiantado

```c#

using var db = new Data.ApplicationContext();
var pedido = db.Pedidos
    .Include(p => p.Itens) // Carregamento adiantado
        .ThenInclude(x => x.Produto)
    .ToList();
//var pedido = db.Pedidos.Include("Itens").ToList();

System.Console.WriteLine(pedido.Count);

```

## 5 - update registers in database

```c#

var cliente = db.Clientes.FirstOrDefault();
cliente.Nome = "Cliente alterado 3";

// com essa linha comentada, é gerado somente update set da coluna Nome
//db.Clientes.Update(cliente); // altera todos todas as propriedade

// outra forma de atualizar todas as propriedades
//db.Entry(cliente).State = EntityState.Modified;

db.SaveChanges();


// Desconectado

var cliente = new Cliente {
    Id = 2
};

var clienteDesconectado = new {
    Id = 2,
    Nome = "Cliente desconectado 2",
    Telefone = "11998765432"
};

db.Attach(cliente);
db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);

db.SaveChanges();

```


## 6 - Delete

```c#

var cliente = db.Clientes.Find(3);

db.Clientes.Remove(cliente);
//db.Remove(cliente);
//db.Entry(cliente).State = EntityState.Deleted;            
db.SaveChanges();


// Deleta de forma desconectada. Gera um query delete sem fazer select
var cliente = new Cliente { Id = 4};
db.Entry(cliente).State = EntityState.Deleted;

db.SaveChanges();


```
