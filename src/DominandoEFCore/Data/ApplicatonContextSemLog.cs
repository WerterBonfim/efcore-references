using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore.Data
{
    public class ApplicatonContextSemLog : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var stringDeConexao = "Server=localhost,1433;Database=DominandoEFCore;User Id=sa; Password=!123Senha;";

            optionsBuilder
                .UseSqlServer(stringDeConexao);
        }
    }
}