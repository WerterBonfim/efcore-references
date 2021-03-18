My resource for learning EFCore

I learned through [desenvolvedor.io][dev.io]

Cource author MVP [Rafael Almeida][rafael]

## How to execute

With Docker:
```bash
git clone https://github.com/WerterBonfim/efcore-references.git

cd efcore-references/docker

docker build -t sql-dev
docker container run --rm --name sql -d -p 1433:1433 sql-dev

```



## [Section 6 - Migrations][migrations]
 * 6 - Applaying Migrations
 * 7 - Generate Scripts Sql Idempotent
 * 8 - Rollback Migrations

## [Section 7 - Operations][operations]

* Insert
* Read
* Update
* Delete



[dev.io]:https://desenvolvedor.io/
[rafael]:https://github.com/ralmsdeveloper

[migrations]:howto/07-migracoes.md
[operations]:howto/08-operations.md
