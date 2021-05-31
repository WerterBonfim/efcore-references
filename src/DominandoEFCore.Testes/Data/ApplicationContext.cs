using DominandoEFCore.Testes.Entities;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore.Testes.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            
        }
    }
}