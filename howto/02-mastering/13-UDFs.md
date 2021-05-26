# UDFs User Defined Function

* 1 - O que é UDF
* 2 - Built-In Function
* 3 - Registrando Funções
* 4 - Registrando Funções Via Fluent API
* 5 - Função definada pelo usuário
* 6 - Customizando uma função complexa


## 1 - O que é UDF

Função definida pelo usuário.

Pode retornar 3 tipos de dados, string, int e tabela values

O EF já mapeada funções incorporadas o SqlServer

## 2 - Built-In Function

```c#


// Mapea uma Build-In Function do Sql Server
[DbFunction(name: "LEFT", IsBuiltIn = true)]
public static string Left(string dados, int quantidade)
{
    throw new NotImplementedException();
}

var resultado = db.Livros
    .Select(x => ApplicationContext.Left(x.Titulo, 5));

foreach(var tituloLivro in resultado)
    Console.WriteLine(tituloLivro);

```


## 3 - Registrando Funções

Para registrar as funções é preciso definir isso no metodo OnModelCreating

```c#


// Definindo uma classe responsavel por registar as funções Sql
public class SqlHelperFunctions
{

    // Registra no EFCore as minhas funções criadas
    public static void Registrar(ModelBuilder builder)
    {
        var funcoes = typeof(SqlHelperFunctions)
            .GetMethods()
            .Where(x => MetodoFoiDefinidoComAtributoDbFunction(x));

        foreach (var funcao in funcoes)
            builder.HasDbFunction(funcao);
    }

    private static bool MetodoFoiDefinidoComAtributoDbFunction(MethodInfo informacoesDoMetodo)
    {
        return Attribute
            .IsDefined(informacoesDoMetodo, typeof(DbFunctionAttribute));
    }

    [DbFunction(name: "LEFT", IsBuiltIn = true)]
    public static string Left(string dados, int quantidade)
    {
        throw new NotImplementedException();
    }
}

protected override void OnModelCreating(ModelBuilder modelBuilder)
{    
    SqlHelperFunctions.Registrar(modelBuilder);
}

```



## 4 - Registrando Funções Via Fluent API

```c#

    // OnModelCreating
    modelBuilder
        .HasDbFunction(_sqlFunctionLeft)
        .HasName("LEFT")
        .IsBuiltIn();    

    // Campo privado da classe
    private static MethodInfo _sqlFunctionLeft = typeof(SqlHelperFunctions)
        .GetRuntimeMethod("Left", new[] {typeof(string), typeof(int)});


```


## 5 - Função definada pelo usuário

```c#

// Program

db.Database.ExecuteSqlRaw(@"
    CREATE FUNCTION ConverterParaLetrasMaiusculas(@dados VARCHAR(100))
    RETURNS VARCHAR(100)
    BEGIN
        RETURN UPPER(@dados)
    END
");

var resultado = db.Livros
    .Select(x => SqlHelperFunctions.LetrasMaiusculas(x.Titulo));

    foreach (var titulo in resultado)
        Console.WriteLine(titulo);



// Registrando via FluentAPI
modelBuilder
    .HasDbFunction(_sqlFunctionLetrasMaiusculas)
    .HasName("ConverterParaLetrasMaiusculas")
    .HasSchema("dbo");

// Campo privado
private static MethodInfo _sqlFunctionLetrasMaiusculas = typeof(SqlHelperFunctions)
            .GetRuntimeMethod("LetrasMaiusculas", new[] {typeof(string)});

// Ou através de classes
[DbFunction(name:"ConverterParaLetrasMaiusculas",  Schema = "dbo")]
public static string LetrasMaiusculas(string dados)
{
    throw new NotImplementedException();
}

```


## 6 - Customizando uma função complexa

Customizando a função interna do Sql Server DATEDIFF que recebe três parametros,
sendo que o primeiro parametro (datepart) é especial:

"O valor de datepart não pode ser especificado em uma variável, nem como strings entre aspas, ex: 'month'".


```c#

// Definição no C#
public static int DateDiff(string identificador, DateTime dataInicial, DateTime dataFinal)
{
    throw new NotImplementedException();
}

// Customizando a parametrização dos parametros
public static SqlExpression DateDiffTranslation(IReadOnlyCollection<SqlExpression> arguments)
{
    var args = arguments.ToArray();
    var datepart = (SqlConstantExpression) args[0];
    var newArgs = new[]
    {
        new SqlFragmentExpression(datepart.Value.ToString()),
        args[1],
        args[2]
    };

    return new SqlFunctionExpression(
        "DATEDIFF",
        newArgs,
        false,
        new[] {false, false, false},
        typeof(int),
        null
    );
}

builder
    .HasDbFunction(ObterMethodInfo(nameof(DateDiff)))
    .HasName("DATEDIFF")
    .HasTranslation(x => DateDiffTranslation(x))
    .IsBuiltIn();

```


