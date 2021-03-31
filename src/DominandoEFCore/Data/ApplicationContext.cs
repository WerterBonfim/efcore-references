using System;
using System.IO;
using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore.Data
{
    public class ApplicationContext : DbContext
    {
        private readonly string _logEF = "/home/werter/Documents/dev-logs/efcore/ef_log.txt";
        private readonly StreamWriter _writer;
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        public ApplicationContext()
        {
            _writer = new StreamWriter(_logEF, true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var stringDeConexao = "Server=localhost,1433;" +
                                  "Database=DominandoEFCore;" +
                                  "User Id=sa; " +
                                  "Password=!123Senha;pooling=true;";

            optionsBuilder
                .UseSqlServer(stringDeConexao, x => x
                    .MaxBatchSize(50)
                    .CommandTimeout(5)
                    .EnableRetryOnFailure(4, TimeSpan.FromSeconds(10), null)
                )
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information)
                ;
        }

        public override void Dispose()
        {
            base.Dispose();
            _writer.Dispose();
        }
    }
}