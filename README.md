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


# Mastering to Entity Framework Core section



[dev.io]:https://desenvolvedor.io/
[rafael]:https://github.com/ralmsdeveloper

[level01.migrations]:howto/01-introducton/07-migracoes.md
[level01.operations]:howto/01-introducton/08-operations.md


