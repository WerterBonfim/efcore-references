using DominandoEFCore.Multitenant.Domain;
using DominandoEFCore.Multitenant.Provider;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore.Multitenant.Data
{
    public class ApplicationContext : DbContext
    {
        private readonly TenantData _tenant;
        public DbSet<Person> People { get; set; }
        public DbSet<Product> Products { get; set; }

        public ApplicationContext()
        {
            
        }

        public ApplicationContext(
            DbContextOptions<ApplicationContext> options,
            TenantData tenant = null
            ) : base(options)
        {
            _tenant = tenant;
        }

        // Estrategia 1
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<Person>().HasData(
        //         new Person { Id = 1, Name = "Person 1", TenantId = "tenant-1"},
        //         new Person { Id = 2, Name = "Person 2", TenantId = "tenant-2"},
        //         new Person { Id = 3, Name = "Person 3", TenantId = "tenant-2"}
        //     );
        //
        //     modelBuilder.Entity<Product>().HasData(
        //         new Product { Id = 1, Description = "Description 1", TenantId = "tenant-1"},
        //         new Product { Id = 2, Description = "Description 2", TenantId = "tenant-2"},
        //         new Product { Id = 3, Description = "Description 3", TenantId = "tenant-2"}
        //     );
        //
        //     modelBuilder.Entity<Person>().HasQueryFilter(p => p.TenantId == _tenant.TenantId);
        //     modelBuilder.Entity<Product>().HasQueryFilter(p => p.TenantId == _tenant.TenantId);
        //
        //
        // }
    }
}