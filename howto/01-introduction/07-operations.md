# Session 8 - Operations

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
