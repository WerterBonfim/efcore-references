using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DominandoEFCore.Multitenant.Data
{
    public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder
                .UseSqlServer("Server=localhost, 1433;Database=TenantDev;User Id=sa;Password=!123Senha;Application Name=\"CursoEFCore\";pooling=true;")
                .LogTo(Console.WriteLine)
                .EnableSensitiveDataLogging();
            
            return new ApplicationContext(builder.Options);
        }
    }
}