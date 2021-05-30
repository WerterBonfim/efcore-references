# Padrão Repository & UoW

* 1 - O que é Repository Pattern?
* 2 - O que é Unit-of-work?
* 3 - DbContext já é um padrão Repository & Uow
* 4 - Preparando o ambiente
* 5 - Implementando UoW - Estratégia 1
* 6 - Implementando UoW - Estragédia 2
* 7 - Criando um Repositório Genérico
* 8 - Consulta com Repositório Genérico 


## 1 - O que é Repository Pattern?

Faz a mediação entre o domínio e as camadas de mapeamento de dados
usando uma interface semelhante a uma coleção para acessar objetos de domínio.
(Martin Fowler)

Basicamente é um objeto que faz o isolamento das entidade do domínio de seu código que faz acesso à dados.

Prós
* Um único ponto de acesso à dados.
* Encapsulamento da lógica de acesso à dados
* SPOF (Ponto único de falha)

Contras
* Maior complexidade

## 2 - O que é Unit-of-work?

Podemos definir Unidade de trabalho(Unit of Work) como uma única transação
que pode envolver múltiplas operações tais como: (inserção/atualização/exclusão).

Mantém uma lista de objetos de negócio afetados por uma transação e coordena a 
escrita a partir de alterações e a resolução de problemas de concorrência. (Martin Fowler)

## 3 - DbContext já é um padrão Repository & Uow

### Vantagens:

* Testabilidade, dados que a gente consegue criar testes "mockados" para testar
parte isolada de regras de negócio.

* Acesso misto ao acesso a dados.

### Desvantagens:
* Complexidade.


## 4 - Preparando o ambiente



## 5 - Implementando UoW - Estratégia 1



## 6 - Implementando UoW - Estragédia 2



## 7 - Criando um Repositório Genérico



## 8 - Consulta com Repositório Genérico 


