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
```


## 6 - Customizando uma função complexa

```c#
```


