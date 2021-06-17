# Modelo de dados

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

## 1 - Colletions

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


## 2 - Sequências
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

## 3 - Índices
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


## 4 - Propagação de dados
```c#
modelBuilder.Entity<Estado>().HasData(new[]
{
    new Estado {Id = 1, Nome = "São Paulo"},
    new Estado {Id = 2, Nome = "Sergipe"}
});
```

## 5 - Esquemas
```c#
modelBuilder.HasDefaultSchema("cadastros");
modelBuilder.Entity<Estado>().ToTable("Estados", "SegundoEsquema");
```


## 6 - Conversor de valores
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


## 7 - Criar um conversor de valor customizado
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

## 8 - Propriedades de sombra
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

## 9 - Owned Types - [IMPORTANTE]
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

## 10 - Relacionamento 1 x 1

Um estado é governado por apenas um governador.
E um governandor governa apenas um estado.

```c#
public void Configure(EntityTypeBuilder<Estado> builder)
{
    builder
        .HasOne(p => p.Governador)
        .WithOne(p => p.Estado)
        .HasForeignKey<Governador>(p => p.EstadoId);

    // Auto Include 
    builder.Navigation(p => p.Governador)
        .AutoInclude();
}

// Main
 var estados = db.Estados
    .AsNoTracking()
    .ToList();

    estados.ForEach(x =>
    {
        Console.WriteLine($"Estado: {x.Nome}, Governador: {x.Governador.Nome}");
    });
```

## 11 - Relacionamento 1 x N



### Configuração de propriedade de navegação única:

```C#

// Model 
public class Estado
{
    public int Id { get; set; }
    public string Nome { get; set; }

    public Governador Governador { get; set; }

    // Configuração de propriedade de navegação única:
    // Não a uma propriedade Estado na classe Cidade
    public ICollection<Cidade> Cidades { get; } = new List<Cidade>();
}

// Model sem a propriedade de navegação
public class Cidade
{
    public int Id { get; set; }
    public string Nome { get; set; }
}

// EstadoConfiguration IEntityTypeConfiguration
builder
    .HasMany(p => p.Cidades)
    .WithOne(); // O EFCore é capaz de fazer o relacionamento


```
Query gerada pelo EF:
Por default ele define On Delete no action


![foto3][OneToMany-NavegacaoUnica]



### Propriedades de navegação:

```C#

public class Estado
{
    public int Id { get; set; }
    public string Nome { get; set; }

    public Governador Governador { get; set; }
    
    public ICollection<Cidade> Cidades { get; } = new List<Cidade>();
}

public class Cidade
{
    public int Id { get; set; }
    public string Nome { get; set; }

    public int EstadoId { get; set; }
    public Estado Estado { get; set; }
}


// IEntityTypeConfiguration
builder
    .HasMany(p => p.Cidades)
    .WithOne(x => x.Estado);
    
    // Mudando o comportamento 
    //.OnDelete(DeleteBehavior.Restrict)
    // Permite inserir uma cidade sem precisar
    // ter um Estado atribuido
    //.IsRequired(false)
    




```

Query gerada pelo EF: Por default ele gera um DELETE CASCADE
![foto4][OneToMany-ComNavegacao]


[OneToMany-NavegacaoUnica]: imgs/OneToMany-Navegacao-Unica.png
[OneToMany-ComNavegacao]: imgs/OneToMany-Com-Navegacao.png

## 12 - Relacionamento N x N
```C#
    public class Ator
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        // Obrigatório em relacionamento NxN para o EF fazer a descoberta (discovery)
        public ICollection<Filme> Filmes { get; } = new List<Filme>();
    }

    public class Filme
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        // Obrigatório em relacionamento NxN para o EF fazer a descoberta (discovery)
        public ICollection<Ator> Atores { get; } = new List<Ator>();
    }


    // Ou via FluentAPI

    public class AtorFilmeConfiguration : IEntityTypeConfiguration<Ator>
    {
        public void Configure(EntityTypeBuilder<Ator> builder)
        {
            builder
                .HasMany(p => p.Filmes)
                .WithMany(p => p.Atores)
                // Opcional para definir o nome da tabela
                .UsingEntity(p => p.ToTable("AtoresFilmes"));
        }
    }

```

## 13 - Customizando muitos-para-muitos
```C#
public class AtorFilmeConfiguration : IEntityTypeConfiguration<Ator>
{
    public void Configure(EntityTypeBuilder<Ator> builder)
    {

        builder
            .HasMany(p => p.Filmes)
            .WithMany(p => p.Atores)
            .UsingEntity<Dictionary<string, object>>("filmesAtores",
                p => p.HasOne<Filme>().WithMany().HasForeignKey("FilmeId"),
                p => p.HasOne<Ator>().WithMany().HasForeignKey("AtorId"),
                // Shadow Property
                p =>
                {
                    p.Property<DateTime>("CadastradoEm").HasDefaultValueSql("GETDATE()");
                }
            );
    }
}
```

## 14 - Campo de apoio (Backing field)
```C#
public class Documento
{
    private string _cpf;

    public int Id { get; set; }

    public void DefinirCpf(string cpf)
    {
        // Validações
        _cpf = cpf;
    }

    public string Cpf => _cpf;
    // Ou
    //public string GetCpf() => _cpf;
}

// IEntityTypeConfiguration
 builder
    .Property(x => x.Cpf)
    .HasField("_cpf");


builder
    .Property("_cpf")
    .HasColumnName("Cpf")
    .HasMaxLength(11);
    //.HasField("_cpf");    


```

## 15 - Configurando modelo de dados com TPH (Tabela por hierarquia)

O EF Core cria um coluna chamada descriminator

```C#
public class Pessoa
{
    public int Id { get; set; }
    public string Nome { get; set; }

    public override string ToString()
    {
        return $"Id:{Id} Nome: {Nome}";
    }
}

public class Instrutor : Pessoa
{
    public DateTime Desde { get; set; }
    public string Tecnologia { get; set; }

    public override string ToString()
    {
        return base.ToString() + $" Tecnologia: {Tecnologia} Desde: {Desde.ToString()}";
    }
}

public class Aluno : Pessoa
{
    public int Idade { get; set; }
    public DateTime DataContrato { get; set; }

    public override string ToString()
    {
        return base.ToString() + $"Idade: {Idade} Data contrato: {DataContrato.ToString()}";
    }
}
```

Com as entidades modelada desta forma, o EF criar as tabelas da seguinte forma:
![foto5][TPH]

Customizando:

```C#

public class PessoaConfiguration : IEntityTypeConfiguration<Pessoa>
{
    public void Configure(EntityTypeBuilder<Pessoa> builder)
    {
        builder
            .ToTable("Pessoas")
            .HasDiscriminator<int>("TipoPessoa")
            .HasValue<Pessoa>(1)
            .HasValue<Instrutor>(2)
            .HasValue<Aluno>(8);
    }
}


// Formas de efetuar a consulta
var alunos = db.Alunos.AsNoTracking().ToArray();
var alunos = db.Pessoas.OfType<Aluno>().AsNoTracking().ToArray();

```

[TPH]: imgs/TPH.png

## 16 - Configurando modelo de dados do TPT (Tabela por tipo)

Adicionado na versão 5.0

Com essa configuração é criada um tabela para cada Tipo/Entidade

```C#
public void Configure(EntityTypeBuilder<Pessoa> builder)
{
    builder.ToTable("Pessoas");    
}

public void Configure(EntityTypeBuilder<Instrutor> builder)
{
    builder.ToTable("Instrutores");
}

public void Configure(EntityTypeBuilder<Aluno> builder)
{
    builder.ToTable("Alunos");
}

```


## 17 - Sacola de propriedades (Property Bags)
```c#

Adicionado na versão 5.0

public DbSet<Dictionary<string, object>> Configuracoes => Set<Dictionary<string, object>>("Configuracoes");


modelBuilder.SharedTypeEntity<Dictionary<string, object>>("Configuracoes", b =>
{
    b.Property<int>("Id");

    b.Property<string>("Chave")
        .HasColumnType("varchar(40)")
        .IsRequired();
    
    b.Property<string>("Valor")
        .HasColumnType("varchar(255)")
        .IsRequired();
});

```

## 18 (extra) - Relationship 1 x N Optional


```c#
public class Order : Entity {
    
    // Nuablle define has a optional relationships
    public Guid? VoucherId { get; get; }
    public Voucher Voucher { get; set; }
}

public class Voucher : Entity {    
    
    public ICollection<Pedido> Pedidos { get; set; }
}




// voucher mapping
builder.HasMany(c => c.Pedidos)
    .WithOne(x => x.Voucher)
    .HasForeignKey(x => x.VoucherId);
```


