# Transações

* 1 - O que é trasação
* 2 - Comportamento padrão do EF Core
* 3 - Gerenciando transação manualmente
* 4 - Revertendo uma transação
* 5 - Salvando ponto de uma transação
* 6 - Usando TransactionScope


## 1 - O que é transação

É uma sequência de operações que serão executadas.

### ACID
 * Atomicty: Faz tudo ou não faz nada
 * Consistency: Todos os dados devem ser validos.
 * Isolation: Uma transação não deve inteferir ou ser afetada por outras transações.
 * Durability: Persistencia na memória permanente (Disco)

## 2 - Comportamento padrão do EF Core

Por padrão o EFCore, através do SaveChanges, envolve as operações em uma transação,
você pode ver essas operações através de uma ferramenta de profile.

No Windows: Sql Profile
No Linux: Azure Data Studio - Extenção "SQL Server Profiler"


## 3 - Gerenciando transação manualmente



```c#

// Internamente o EF Core irar interagir com o driver do sql server
// e irar gerar um instancia do DbTransaction (ADO.NET)
var transacao = db.Database.BeginTransaction();

var livro = db.Livros.FirstOrDefault(x => x.Id == 1);
livro.Autor = "Werter TDD";

// Irar interagir com a transação
db.SaveChanges();

// Para visualizar no Sql e Sql Profile o que acontece
Console.ReadKey();


db.Livros.Add(new Livro
{
    Titulo = "Boas praticas e código limpo",
    Autor = "Werter"
});

db.SaveChanges();


// Efetiva as alterações no banco de dados
transacao.Commit();
```


## 4 - Revertendo uma transação
```c#
var transacao = db.Database.BeginTransaction();
try
{
    var livro = db.Livros.FirstOrDefault(x => x.Id == 1);
    // Forçando um erro, a coluna autor tem um tamanho maximo de 
    livro.Autor = "Werter TDD".PadLeft(16, '*');
    db.SaveChanges();    
    transacao.Commit();
}
catch (Exception e)
{
    // Revertendo as alterações
    transacao.Rollback();
}
```


## 5 - Salvando ponto de uma transação
```c#

```


## 6 - Usando TransactionScope
```c#
```

