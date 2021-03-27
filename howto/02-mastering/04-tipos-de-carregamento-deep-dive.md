# 04 Tipos de carregamentos - DeepDive

* 1 - Adiantado (Eager)
* 2 - Explícito
* 3 - Lento (Famoso LazyLoad)


## 1 - Adiantado (Eager)

Departamento/Funcionarios

Departamento possui muitos Funcionarios

Um Funcionario possui um departamento.

Força o relacionamento trazendo os dados das propriedades de navegação.
E um otimo recurso para quando a poucos dados a ser retornado.

TradeOff: Se ouver muito dados pode haver uma explosão cartesiana. As linhas do lado unico sempre serão duplicadas para os registro correspondentes no lado de muitos. Para cada funcionario sera carregando novamente um departamento.

```c#
var departamentos = db
    .Departamentos
    .Include(p => p.Funcionarios);
```



## 2 - Explícito
```
```

```
```

## 3 - Lento (Famoso LazyLoad)
```
```

```
```
