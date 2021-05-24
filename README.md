My resource for learning EFCore

I learned through:
* [desenvolvedor.io][dev.io] | Cource author MVP [Rafael Almeida][rafael]
* 


[dev.io]:https://desenvolvedor.io/
[rafael]:https://github.com/ralmsdeveloper


EFCore version 5.0.4

## How to execute

I'm using Linux to developer this tutorial

With Docker:
```bash
git clone https://github.com/WerterBonfim/efcore-references.git

cd efcore-references/docker

docker build -t sql-dev .
docker container run --rm --name sql -d -p 1433:1433 sql-dev

```

# Introduction to Entity Framework Core section

## [Section 6 - Migrations][level01.migrations]

 * 6 - Applaying Migrations
 * 7 - Generate Scripts Sql Idempotent
 * 8 - Rollback Migrations

 [level01.migrations]:howto/01-introduction/06-migracoes.md

## [Section 7 - Operations][level01.operations]

* 1 - Insert
* 2 - Read
* 3 - Update
* 4 - Delete

[level01.operations]:howto/01-introduction/07-operations.md

## [Section 8 - Bonus][level01.bonus]
* 1 - Deletando propriedades não configuradas
* 2 - Resiliência da conexão
* 3 - Alterando o nome da tabela de histórico de migrações

[level01.bonus]:howto/01-introduction/08-bonus.md


# Mastering to Entity Framework Core section

## [Section 2 - Setup][level02.setup]

[level02.setup]:howto/02-mastering/02-setup.md

## [Section 3 - EF Database][level02.ef-database]

* 1 - Ensure Created/Deleted
* 2 -  Resolver GAP do Ensure Created
* 3 -  HealthCheck do Banco de dados
* 4 -  Gerenciar estado da conexão
* 5 -  Commandos ExecuteSql
* 6 -  Como se proteger de ataques Sql Injection
* 7 -  Detectando migraçoes pendentes
* 8 -  Forçando uma migração
* 9 -  Recuperando migrações existentes em sua aplicação
* 10 - Recuperando migrações aplicadas em seu banco de dados
* 11 - Gerar Script Sql de seu modelo de dados


[level02.ef-database]:howto/02-mastering/03-ef-database.md

## [Section 4 - Tipos de carregamento - DeepDive][level02.tipos-de-carregamento]

* 1 - Carregamento Adiantado (Eager)
* 2 - Carregamento Explícito
* 3 - Carregamento Lento (Famoso LazyLoad)

[level02.tipos-de-carregamento]:howto/02-mastering/04-tipos-de-carregamento-deep-dive.md


## [Section 5 - Consultas ][level02.consultas]

* 1 - Configurando um filtro global
* 2 - Ignorando filtros globais
* 3 - Consultas projetadas
* 4 - Consulta parametrizada
* 5 - Consulta interpolada
* 6 - Usando o recurso TAG em consultas
* 7 - Entendendo diferença em consulta 1xN vs Nx1
* 8 - Divisão de consultas com SplitQuery


[level02.consultas]:howto/02-mastering/05-consultas.md


## [Section 6 - Stored Procedures ][level02.stored-procedures]

* 1 - Criando uma procedure de inserção
* 2 - Executando uma inserção via procedure
* 3 - Criando uma procedure de consulta
* 4 - Executando uma consulta via procedure


[level02.stored-procedures]:howto/02-mastering/06-stored-procedure.md


## [Section 7 - Infraestrutura ][level02.infraestrutura]

* 1 - Configurando log simplificado
* 2 - Filtrado categoria de log
* 3 - Escrevendo log em um arquivo
* 4 - Habilitando erros detalhados
* 5 - Habilitando visualização dos dados sensíveis
* 6 - Configurando o batch size
* 7 - Configurando o timeout do comando global
* 8 - Configurando o timeout para um comando para um fluxo
* 9 - Confgurando resiliência para sua aplicação
* 10 - Criando uma estratégia de resiliência


[level02.infraestrutura]:howto/02-mastering/07-infraestrutura.md

## [Section 8 - Modelo de dados ][level02.modelo-de-dados]

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

[level02.modelo-de-dados]:howto/02-mastering/08-modelo-de-dados.md
## [ Section 9 - Atributos - Data Annotations ][level02.data-annotations]

* 1 - Atributo Table
* 2 - Atributo Inverse Property
* 3 - Atributo NotMapped
* 4 - Atributo Database Generated
* 5 - Atributo Index
* 6 - Atributo Comment
* 7 - Atributo Backing Field
* 8 - Atributo Keyless

[level02.data-annotations]:howto/02-mastering/09-data-annotations.md


## [Section 10 - EF Functions ][level02.ef-functions]

* 1 - Funções de datas
* 2 - Funções Like
* 3 - Funções DataLenght
* 4 - Funções Property
* 5 - Funções Collate

[level02.ef-functions]:howto/02-mastering/10-ef-functions.md

## [Section 11 - Interceptação][level02.interceptação]

* 1 - O que são interceptadores de comandos?
* 2 - Criando e registrando um interceptador
* 3 - Sobrescrevendo métodos da classe base
* 4 - Aplicando hint NOLOCK nas consultas
* 5 - Interceptando abertura de conexão com o banco
* 6 - Interceptando alterações.


[level02.interceptação]:howto/02-mastering/11-interceptação.md
