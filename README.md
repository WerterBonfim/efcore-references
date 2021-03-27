My resource for learning EFCore

I learned through:
* [desenvolvedor.io][dev.io] | Cource author MVP [Rafael Almeida][rafael]
* 




EFCore version 5.0.3

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

## [Section 7 - Operations][level01.operations]

* 1 - Insert
* 2 - Read
* 3 - Update
* 4 - Delete

## [Section 8 - Bonus][level01.bonus]
* 1 - Deletando propriedades não configuradas
* 2 - Resiliência da conexão
* 3 - Alterando o nome da tabela de histórico de migrações


# Mastering to Entity Framework Core section

## [Section 2 - Setup][level02.setup]


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


[dev.io]:https://desenvolvedor.io/
[rafael]:https://github.com/ralmsdeveloper

[level01.migrations]:howto/01-introduction/06-migracoes.md
[level01.operations]:howto/01-introduction/07-operations.md
[level01.bonus]:howto/01-introduction/08-bonus.md

[level02.setup]:howto/02-mastering/02-setup.md
[level02.ef-database]:howto/02-mastering/03-ef-database.md
