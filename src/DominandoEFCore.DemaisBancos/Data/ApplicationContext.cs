using System;
using DominandoEFCore.DemaisBancos.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore.DemaisBancos.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Pessoa> Pessoas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //const string strConnectionPg = "Host=localhost;Database=DominandoEFCore;Username=postgres;Password=123";

            optionsBuilder
                //.UseNpgsql(strConnectionPg)
                //.UseSqlite("DataSource=DominandoEFCore.db")
                //.UseInMemoryDatabase(databaseName: "DominandoEFCore")
                .UseCosmos(
                    accountEndpoint: "https://localhost:8081",
                    accountKey: "",
                    databaseName: "DominandoEFCore"
                )
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pessoa>(c =>
            {
                c.HasKey(x => x.Id);
                c.Property(x => x.Nome)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                c.ToContainer("Pessoas");
            });
        }
    }
}