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
```
```

```
```
## 3 - Funções DataLenght
```
```

```
```
## 4 - Funções Property
```
```

```
```
## 5 - Funções Collate
```
```

```
```
