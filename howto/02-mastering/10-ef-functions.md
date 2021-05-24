# EF Functions

* 1 - Funções de datas
* 2 - Funções Like
* 3 - Funções DataLenght
* 4 - Funções Property
* 5 - Funções Collate



## 1 - Funções de datas

A classe EF do namespace Microsoft.EntityFrameworkCore.EF fornece uma propriedade chamada
Function, através dela é oferecido uma serie de metodos de estenção estaticos que mapea as funções do sql server

```C#
var dados = db.Funcoes
        .AsNoTracking()
        .Select(x => new
        {
            Dias = EF.Functions.DateDiffDay(DateTime.Now, x.Data1),
            Data = EF.Functions.DateFromParts(2021, 5, 21),
            DataValida = EF.Functions.IsDate(x.Data2)
        });

    foreach (var d in dados)
        Console.WriteLine(d);
```

## 2 - Funções Like
```C#
var dados = db
    .Funcoes
    .AsNoTracking()
    .Where(x => EF.Functions.Like(x.Descricao1, "B[ao]%"))
    .Select(x => x.Descricao1)
    .ToArray();
```


## 3 - Funções DataLenght

Função para obter quantos bytes foram alocados no banco de dados.

```c#
 var resultado = db
        .Funcoes
        .AsNoTracking()
        .Select(x => new
        {
            TotalBytesCampoData = EF.Functions.DataLength(x.Data1),
            TotalBytes1 = EF.Functions.DataLength(x.Descricao1),
            TotalBytes2 = EF.Functions.DataLength(x.Descricao2),
            Total1 = x.Descricao1.Length,
            Total2 = x.Descricao2.Length
        })
        .FirstOrDefault();
```

```text
Resultado:
{ TotalBytesCampoData = 8, TotalBytes1 = 12, TotalBytes2 = 6, Total1 = 6, Total2 = 6 }

```
## 4 - Funções Property
```C#
// As propriedades de sombra não existe no tipo CLR da entidade Funcao.
// As propriedade de sobra são somente associadas a cada registro se a  
// consulta for rastreada

var resultado = db
    .Funcoes
    //.AsNoTracking()
    .FirstOrDefault(x => EF.Property<string>(x, "PropriedadeSombra") == "Teste");

var propriedadeSombra = db
    .Entry(resultado)
    .Property<string>("PropriedadeSombra")
    .CurrentValue;

Console.WriteLine("Resultado:");
Console.WriteLine(propriedadeSombra);
```

## 5 - Funções Collate
```C#
var caseSensitive = "SQL_Latin1_General_CP1_CS_AS";
var caseInsensitive = "SQL_Latin1_General_CP1_CI_AS";

var consultaCS = db
    .Funcoes
    .FirstOrDefault(x => EF.Functions.Collate(x.Descricao2, caseSensitive) == "tela");

var consultaCI = db
    .Funcoes
    .FirstOrDefault(x => EF.Functions.Collate(x.Descricao2, caseInsensitive) == "tela");

// Output:
// Consulta 1 CS: 
// Consulta 2 CI: Tela

```

