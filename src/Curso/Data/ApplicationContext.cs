using System;
using System.Linq;
using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CursoEFCore.Data
{
    public class ApplicationContext : DbContext
    {

        private static readonly ILoggerFactory _logger =
            LoggerFactory.Create(p => p.AddConsole());

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder
                .UseLoggerFactory(_logger)
                .EnableSensitiveDataLogging()
                .UseSqlServer("Server=localhost,1433;Database=CursoEFCore;User Id=sa;Password=!123Senha;",
                    p => p
                        .EnableRetryOnFailure(
                            maxRetryCount: 2, // qtd de tentativa
                            maxRetryDelay: TimeSpan.FromSeconds(5), // Aguardar 5 segundos após o erro
                            errorNumbersToAdd: null)
                        .MigrationsHistoryTable("curso_ef_core")
                );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region [ modelBuilder ]

            // modelBuilder.Entity<Cliente>(p =>
            // {
            //     p.ToTable("Clientes");
            //     p.HasKey(p => p.Id);
            //     p.Property(p => p.Nome).HasColumnType("VARCHAR(80)").IsRequired();
            //     p.Property(p => p.Telefone).HasColumnType("CHAR(11)").IsRequired();
            //     p.Property(p => p.CEP).HasColumnType("CHAR(80)").IsRequired();
            //     p.Property(p => p.Estado).HasColumnType("CHAR(2)").IsRequired();
            //     p.Property(p => p.Cidade).HasMaxLength(60).IsRequired();

            //     p.HasIndex(i => i.Telefone).HasName("idx_cliente_telefone");

            // });

            // modelBuilder.Entity<Produto>(p =>
            // {
            //     p.ToTable("Produtos");
            //     p.HasKey(p => p.Id);
            //     p.Property(p => p.CodigoBarras).HasColumnType("VARCHAR(14)").IsRequired();
            //     p.Property(p => p.Descricao).HasColumnType("VARCHAR(60)");
            //     p.Property(p => p.Valor).IsRequired();
            //     // Salva o enum com string
            //     p.Property(p => p.TipoProduto).HasConversion<string>();
            // });


            // modelBuilder.Entity<Pedido>(p =>
            // {
            //     p.ToTable("Pedidos");
            //     p.HasKey(p => p.Id);
            //     p.Property(p => p.IniciadoEm).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
            //     p.Property(p => p.Status).HasConversion<string>();
            //     p.Property(p => p.TipoFrete).HasConversion<int>();
            //     p.Property(p => p.Observacao).HasColumnType("varchar(512)");


            //     p.HasMany(p => p.Itens)
            //         .WithOne(p => p.Pedido)
            //         // Os itens devem ser deletados primeiros
            //         //.OnDelete(DeleteBehavior.Restrict)
            //         // Deleta todos os itens do pedido
            //         .OnDelete(DeleteBehavior.Cascade);

            // });

            // modelBuilder.Entity<PedidoItem>(p =>
            // {
            //     p.ToTable("PedidoItens");
            //     p.HasKey(p => p.Id);
            //     p.Property(p => p.Quantidade).HasDefaultValue(1).IsRequired();
            //     p.Property(p => p.Valor).IsRequired();
            //     p.Property(p => p.Desconto).IsRequired();

            // });

            #endregion

            //modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
        }

        private void MapearPropriedadesEsquecidas(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entity.GetProperties()
                    .Where(x => x.ClrType == typeof(string));

                foreach (var property in properties)
                {
                    var maxLenghtIsNotDefined =
                        string.IsNullOrEmpty(property.GetColumnType()) &&
                        !property.GetMaxLength().HasValue;

                    if (maxLenghtIsNotDefined)
                    {

                        //property.SetMaxLength(100);
                        property.SetColumnType("VARCHAR(100)");

                    }
                }
            }
        }




    }
}
