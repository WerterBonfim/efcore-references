using System;
using System.Linq;
using DominandoEFCore.DicasTruques.Domain;
using DominandoEFCore.DicasTruques.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore.DicasTruques.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<UsuarioFuncao> UsuarioFuncaos { get; set; }
        public DbSet<DepartamentoRelatorio> DepartamentoRelatorios { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string stringDeConexao =
                "Server=localhost, 1433;Database=DominandoEFCore;User Id=sa;Password=!123Senha;Application Name=\"CursoEF\";pooling=true;";
            
            optionsBuilder
                .UseSqlServer(stringDeConexao)
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<DepartamentoRelatorio>(e =>
            {
                e.HasNoKey();

                e.ToView("vw_departamento_relatorio");

                e.Property(x => x.Departamento)
                    .HasColumnName("Descricao");
            });


            var camposNVarchar = modelBuilder.Model.GetEntityTypes()
                .SelectMany(x => x.GetProperties())
                .Where(x => x.ClrType == typeof(string) && x.GetColumnType() == null);

            foreach (var property in camposNVarchar)
                property.SetIsUnicode(false);
            
            modelBuilder.ToSnakeCaseNames();

        }
    }
}