using CursoEFCore.Data.Configuration;
using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;

namespace CursoEFCore.Data
{
    public class ApplicationContext : DbContext
    {

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("Server=localhost,1433;Database=CursoEFCore;User Id=sa;Password=!123Senha;");
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





    }
}
