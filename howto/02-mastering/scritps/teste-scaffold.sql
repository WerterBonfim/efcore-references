create database EFCoreScaffoldExemplo collate SQL_Latin1_General_CP1_CI_AS
go

use EFCoreScaffoldExemplo
go

grant connect on database :: EFCoreScaffoldExemplo to dbo
go

grant view any column encryption key definition, view any column master key definition on database :: EFCoreScaffoldExemplo to [public]
go

create table Produtos
(
	Id int identity
		constraint Produtos_pk
			primary key nonclustered,
	Nome varchar(60),
	Preco decimal(6,2),
	Descricao varchar(200),
	DataHoraCadastro datetime
)
go

exec sp_addextendedproperty 'MS_Description', 'Comentario qualquer para ver como o EF se comporta', 'SCHEMA', 'dbo', 'TABLE', 'Produtos'
go

create unique index Produtos_Id_uindex
	on Produtos (Id)
go

create table Usuarios
(
	Id int identity
		constraint Usuarios_pk
			primary key nonclustered,
	Nome varchar(30),
	CPF varchar(12) not null,
	email varchar(20) not null
)
go

exec sp_addextendedproperty 'MS_Description', 'Comentario da tabela de usuarios', 'SCHEMA', 'dbo', 'TABLE', 'Usuarios'
go

create unique index Usuarios_Id_uindex
	on Usuarios (Id)
go
