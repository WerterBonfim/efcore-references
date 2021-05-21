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
    public class Voo
    {
        
    }

```

## 4 - Atributo Database Generated
```
```

```
```

## 5 - Atributo Index
```
```

```
```

## 6 - Atributo Comment
```
```

```
```

## 7 - Atributo Backing Field
```
```

```
```

## 8 - Atributo Keyless
```
```

```
```

