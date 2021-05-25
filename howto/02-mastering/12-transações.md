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

var transacao = db.Database.BeginTransaction();
try
{
    var livro = db.Livros.FirstOrDefault(x => x.Id == 1);
    livro.Autor = "Werter do TDD";
    db.SaveChanges();
    
    transacao.CreateSavepoint("desfazer_apenas_insercao");

    Console.ReadKey();

    db.Livros.Add(new Livro
    {
        Titulo = "Boas praticas e código limpo",
        Autor = "Werter"
    });

    db.SaveChanges();

    // Da um erro aqui
    db.Livros.Add(new Livro
    {
        Titulo = "Qualquer um",
        Autor = "Vai dar erro".PadLeft(16, '*')
    });
    
    db.SaveChanges();

    transacao.Commit();
}
catch (DbUpdateException e)
{
    transacao.RollbackToSavepoint("desfazer_apenas_insercao");

    var todasEntidadesSaoDeInclusao = 
        e.Entries
            .Count(x => x.State == EntityState.Added) == e.Entries.Count;
    
    if (todasEntidadesSaoDeInclusao) transacao.Commit();
    
}

```


## 6 - Usando TransactionScope

O TransactionScope é uma classe que faz transações com base em um bloco de código, ela possui
a própria infraestrutura para gerencias as transações automaticamente.

Sendo assim o TransactionScoped reduz a complexidade do código e é muito utilizado e indicado.



```c#

var options = new TransactionOptions
{
    IsolationLevel = IsolationLevel.ReadCommitted
};

using var scope = new TransactionScope(TransactionScopeOption.Required, options);
ConsultarAtualizar();
CadastrarLivroCodigoLimpo();
CadastrarLivroDominandoEFCore();

scope.Complete();
```

