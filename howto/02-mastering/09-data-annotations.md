# Atributos - Data Annotations

* 1 - Atributo Table
* 2 - Atributo Inverse Property
* 3 - Atributo NotMapped
* 4 - Atributo Database Generated
* 5 - Atributo Index
* 6 - Atributo Comment
* 7 - Atributo Backing Field
* 8 - Atributo Keyless


## 1 - Atributo Table
```C#
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("TabelaAtributos")]
public class Atributo
{
    [Key] 
    public int Id { get; set; }

    [Column("MinhaDescrição", TypeName = "VARCHAR(100)")]
    public string Descricao { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Observacao { get; set; }
}
// Program
using var db = new ApplicationContext();
var script = db.Database.GenerateCreateScript();
Console.WriteLine(script);
```


## 2 - Atributo Inverse Property

É usando quando uma entidade tem mais de um relacionamento

```C#

// Exemplo de relação 1xN
public class Aeroporto
{
    public int Id { get; set; }
    public string Nome { get; set; }

    [InverseProperty("AeroportoPartida")]
    public ICollection<Voo> VoosDePartida { get; set; }
    
    [InverseProperty("AeroportoChegada")]
    public ICollection<Voo> VoosDeChegada { get; set; }
}

public class Voo
{
    public int Id { get; set; }
    public string Descricao { get; set; }
    public Aeroporto AeroportoPartida { get; set; }
    public Aeroporto AeroportoChegada { get; set; }
}
```


## 3 - Atributo NotMapped
```C#
 public class Aeroporto
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        // Não ira criar essa coluna na tabela
        [NotMapped]
        public string PropriedadeTeste { get; set; }        
    }

    // Não ira criar esse tabela no banco de dados
    [NotMapped]
    public class Voo  { }

```

## 4 - Atributo Database Generated

Uma coluna na tabela que é manipulada de outros meios, 
você só quer ler esse dado e não alterá-lo. 

```C#

public class Atributo
{
    [Key] 
    public int Id { get; set; }

    [Column("MinhaDescrição", TypeName = "VARCHAR(100)")]
    public string Descricao { get; set; }   
    
    // O EF não irar incluir essa propriedade nas instruções de insert
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [MaxLength(255)]
    public string Observacao { get; set; }
    
    
}

db.Atributos.Add(new Atributo
{
    Descricao = "Exemplo",
    Observacao = "Observação"
});

db.SaveChanges();
```
Resultado do insert:

![foto1][DatabaseGenerated]

[DatabaseGenerated]:imgs/DatabaseGenerated.png


## 5 - Atributo Index



```C#
// O Atributo Index foi criado pela equipe do EF Core
using Microsoft.EntityFrameworkCore;


[Table("TabelaAtributos")]
[Index(nameof(Descricao), nameof(Id), IsUnique = true)]
public class Atributo
{
    [Key] 
    public int Id { get; set; }

    [Column("MinhaDescrição", TypeName = "VARCHAR(100)")]
}
```

output:
```text
CREATE UNIQUE INDEX [IX_TabelaAtributos_MinhaDescrição_Id] ON [TabelaAtributos] ([MinhaDescrição], [Id]) WHERE [MinhaDescrição] IS NOT NULL;
```

## 6 - Atributo Comment
```c#

// O Atributo Comment foi criado pelo Rafael
using Microsoft.EntityFrameworkCore;

[Table("TabelaAtributos")]
[Index(nameof(Descricao), nameof(Id), IsUnique = true)]
[Comment("Meu comentario sobre minha tabela")]
public class Atributo
{
    [Key] 
    public int Id { get; set; }

    [Column("MinhaDescrição", TypeName = "VARCHAR(100)")]
    [Comment("Meu comentario sobre essa coluna")]
    public string Descricao { get; set; }    
    
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [MaxLength(255)]
    public string Observacao { get; set; }
    
    
}
```

Resulado da criação:

![foto2][AtributoComment]

[AtributoComment]:imgs/Atributo-Comment.png

## 7 - Atributo BackingField
```c#
public class Documento
{
    private string _cpf;

    public int Id { get; set; }

    public void DefinirCpf(string cpf)
    {
        // Validações        
    }

    [BackingField(nameof(_cpf))]
    public string CPF => _cpf;    
}
```


## 8 - Atributo Keyless

Configura uma entidade que não possui um achave primaria no 
seu banco de dados. Exemplos: Views, tabelas de projeções.

Essas entidades não são rastreadas pelo EF Core, caso você tente
inserir um registro no banco de dados, o EF ira gerar uma Exception
pois ele não adiciona registros em uma tabela sem ID

```C#
[Keyless]
public class RelatorioFinanceiro
{
    public string Descricao { get; set; }
    public decimal Total { get; set; }
    public DateTime Data { get; set; }
} 

```

