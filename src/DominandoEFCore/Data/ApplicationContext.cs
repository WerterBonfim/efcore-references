using System;
using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var stringDeConexao = "Server=localhost,1433;" +
                                  "Database=DominandoEFCore;" +
                                  "User Id=sa; " +
                                  "Password=!123Senha;pooling=true;";
                                  //"MultipleActiveResultSets=true";

            optionsBuilder
                .UseSqlServer(stringDeConexao)
                .EnableSensitiveDataLogging()
                //.UseLazyLoadingProxies()
                .LogTo(Console.WriteLine, LogLevel.Information)
                ;
        }
    }
}