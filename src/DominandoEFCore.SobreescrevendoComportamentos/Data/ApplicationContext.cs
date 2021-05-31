using System;
using DominandoEFCore.Testes.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DominandoEFCore.SobreescrevendoComportamentos.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                //.LogTo(Console.WriteLine)
                .UseSqlServer("Server=localhost, 1433;Database=DominandoEFCore;User Id=sa;Password=!123Senha;")
                //.EnableSensitiveDataLogging()
                .ReplaceService<IQuerySqlGeneratorFactory, MySqlServerQuerySqlGeneratorFactory>();
        }
    }
}