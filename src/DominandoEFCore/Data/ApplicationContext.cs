using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DominandoEFCore.Configurations;
using DominandoEFCore.Conversores;
using DominandoEFCore.Domain;
using DominandoEFCore.Interceptadores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Conversor> Conversores { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Ator> Atores { get; set; }
        public DbSet<Documento> Documentos { get; set; }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Instrutor> Instrutores { get; set; }
        public DbSet<Aluno> Alunos { get; set; }

        public DbSet<Dictionary<string, object>> Configuracoes => Set<Dictionary<string, object>>("Configuracoes");

        public DbSet<Atributo> Atributos { get; set; }

        public DbSet<Aeroporto> Aeroportos { get; set; }

        public DbSet<Funcao> Funcoes { get; set; }
        public DbSet<Livro> Livros { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var stringDeConexao = "Server=localhost,1433;" +
                                  "Database=DominandoEFCore;" +
                                  "User Id=sa; " +
                                  "Password=!123Senha;" +
                                  "Application Name=\"Rider CursoEFCore\";" +
                                  "pooling=true;";


            optionsBuilder
                .UseSqlServer(stringDeConexao)
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information)
                //.AddInterceptors(new InterceptadorDeConexao())
                .AddInterceptors(new InterceptadorDeComandos())
                .AddInterceptors(new InterceptadorDePercistencia())
                ;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);

            modelBuilder.SharedTypeEntity<Dictionary<string, object>>("Configuracoes", b =>
            {
                b.Property<int>("Id");

                b.Property<string>("Chave")
                    .HasColumnType("varchar(40)")
                    .IsRequired();
                
                b.Property<string>("Valor")
                    .HasColumnType("varchar(255)")
                    .IsRequired();
            });


            modelBuilder
                .Entity<Funcao>(c =>
                {
                    c.Property<string>("PropriedadeSombra")
                        .HasColumnType("VARCHAR(100)")
                        .HasDefaultValueSql("'Teste'");
                });
            
            SqlHelperFunctions.Registrar(modelBuilder);

        }

       
    }
}