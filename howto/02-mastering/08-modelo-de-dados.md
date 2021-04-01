## Modelo de dados

* 1 - Colletions
* 2 - Sequências
* 3 - Índices
* 4 - Propagação de dados
* 5 - Esquemas
* 6 - Conversor de valores
* 7 - Criar um conversor de valor customizado
* 8 - Propriedades de sombra
* 9 - Owned Types
* 10 - Relacionamento 1 x 1
* 11 - Relacionamento 1 x N
* 12 - Relacionamento N x N
* 13 - Campo de apoio (Backing field)
* 14 - TPH e TPT
* 15 - Sacola de propriedades (Property Bags)

### 1 - Colletions

Adicionado na versão 5.0

```c#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AI");

    modelBuilder.Entity<Departamento>()
        .Property(x => x.Descricao)
        .UseCollation("SQL_Latin1_General_CP1_CS_AS");
}
```


### 2 - Sequências
```c#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.HasSequence<int>("MinhaSequencia", "sequencias")
        .StartsAt(1)
        .IncrementsBy(2)
        .HasMin(1)
        .HasMax(10)
        .IsCyclic();

    modelBuilder
        .Entity<Departamento>()
        .Property(x => x.Id)
        .HasDefaultValueSql("NEXT VALUE FOR sequencias.MinhaSequencia");
}
```

### 3 - Índices
```c#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder
        .Entity<Departamento>()
        //.HasIndex(p => p.Descricao);
        .HasIndex(p => new { p.Descricao, p.Ativo})
        .HasDatabaseName("idx_meu_index_composto")
        .HasFilter("Descricao IS NOT NULL")
        .HasFillFactor(80)
        .IsUnique();
}
```


### 4 - Propagação de dados
```c#
modelBuilder.Entity<Estado>().HasData(new[]
{
    new Estado {Id = 1, Nome = "São Paulo"},
    new Estado {Id = 2, Nome = "Sergipe"}
});
```

### 5 - Esquemas
```c#
modelBuilder.HasDefaultSchema("cadastros");
modelBuilder.Entity<Estado>().ToTable("Estados", "SegundoEsquema");
```


### 6 - Conversor de valores
```c#
var conversao =
        new ValueConverter<Versao, string>(p =>
            p.ToString(), p => (Versao) Enum.Parse(typeof(Versao), p));

    var conversao1 = new EnumToStringConverter<Versao>();

    modelBuilder.Entity<Conversor>()
        .Property(x => x.Versao)
        .HasConversion(conversao1)
        //.HasConversion(conversao)        
        //.HasConversion(p => p.ToString(), p => (Versao) Enum.Parse(typeof(Versao), p))
        // Converte de int para string
        //.HasConversion<string>();
        ;
```


### 7 - Criar um conversor de valor customizado
```c#

public class Conversor
{       
    public Status Status { get; set; }    
}

public enum Status
{
    Analise,
    Enviado,
    Devolvido
}

// Envia para o banco somente o primeiro caracter do enum
// ao efetuar uma busca do banco, busca pela primeira letra
public class ConversorCustomizado : ValueConverter<Status, string>
{
    public ConversorCustomizado() : base(
        p => ConverterParaOBancoDeDados(p),
        value => ConverterParaAplicacao(value), new ConverterMappingHints(1))
    {
    }

    static string ConverterParaOBancoDeDados(Status status) => status.ToString()[0..1];

    // 
    static Status ConverterParaAplicacao(string value) =>
        Enum.GetValues<Status>()
            .FirstOrDefault(p => p.ToString()[0..1] == value);
}


modelBuilder.Entity<Conversor>()
    .Property(x => x.Status)
    .HasConversion(new ConversorCustomizado());


// main
var conversorDevolvido = db.Conversores
    .AsNoTracking()
    .FirstOrDefault(p => p.Status == Status.Devolvido);
```

```sql
-- Retorno do EF
SELECT TOP(1) [c].[Id], [c].[Ativo], [c].[EnderecoIP], [c].[Excluido], [c].[Status], [c].[Versao]
FROM [Conversores] AS [c]
WHERE [c].[Status] = N'D'


```

### 8 - Propriedades de sombra
```c#
public class Funcionario
{    
    // Sem definir explicitamente, o EFCore criar uma properiedade
    // oculta/sombra (Shadown Properties)
    //public int DepartamentoId { get; set; }
    public Departamento Departamento { get; set; }
}

// DbContext
 // Configurando Shadow Property
modelBuilder.Entity<Departamento>()
    .Property<DateTime>("UltimaAtualizacao");

// main
var departamento = new Departamento
{
    Descricao = "Departamento Propriedade de sombra"
};

db.Departamentos.Add(departamento);

db.Entry(departamento).Property("UltimaAtualizacao").CurrentValue = DateTime.Now;

db.SaveChanges();

var departamentos = db.Departamentos
    .Where(p => EF.Property<DateTime>(p, "UltimaAtualizacao") < DateTime.Now)
    .ToArray();
Console.WriteLine(departamento.Descricao);
```

### 9 - Owned Types - [IMPORTANTE]
```c#

public class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Telefone { get; set; }
    public Endereco Endereco { get; set; }
    
}

// Objeto de valor
public class Endereco
{
    public string Logradouro { get; set; }
    public string Bairro { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }
}

// No DbContext
modelBuilder.Entity<Cliente>(p =>
{
    p.OwnsOne(x => x.Endereco);
});

```

resultado no Sql:

![foto][ownsOne]


```c#

// Customizando o nome das propriedades
modelBuilder.Entity<Cliente>(p =>
{
    p.OwnsOne(x => x.Endereco, end =>
    {
        end.Property(p => p.Bairro).HasColumnName("Bairro");
    });
});
```

Resultado:

![foto2][OwnsOne-Propriedade]



```c#

// Table split
modelBuilder.Entity<Cliente>(p =>
{
    p.OwnsOne(x => x.Endereco, end =>
    {
        end.Property(p => p.Bairro).HasColumnName("Bairro");

        // Table Split | Shadow Property -> Endereco>ClienteId
        end.ToTable("Endereco");
    });
});
```

Resultado:

![foto2][OwnsOne-table-split]




[ownsOne]: imgs/OwnsOne.png
[OwnsOne-Propriedade]: imgs/OwnsOne-Propriedade.png
[OwnsOne-table-split]: imgs/OwnsOne-table-split.png

### 10 - Relacionamento 1 x 1
```
```

```
```

### 11 - Relacionamento 1 x N
```
```

```
```

### 12 - Relacionamento N x N
```
```

```
```

### 13 - Campo de apoio (Backing field)
```
```

```
```

### 14 - TPH e TPT
```
```

```
```

### 15 - Sacola de propriedades (Property Bags)
```
```

```
```
