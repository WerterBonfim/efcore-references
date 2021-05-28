using System;
using System.Text;
using DominandoEFCore.AulaMigrations.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore.AulaMigrations.Data
{
    public class ApplicationContext : DbContext
    {
        private string _stringDeConexao => new StringBuilder()
            .Append("Server=localhost, 1433;")
            .Append("Database=TreinoMigrations;")
            .Append("User Id=sa;")
            .Append("Password=!123Senha;")
            .Append("Application Name=\"Rider CursoEFCore\";")
            .Append("pooling=true;")
            .ToString();
        
        public DbSet<Pessoa> Pessoas { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(_stringDeConexao)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pessoa>(conf =>
            {
                conf.HasKey(x => x.Id);
                conf.Property(p => p.Nome)
                    .HasMaxLength(60)
                    .IsUnicode(false);
            });
        }
    }
}