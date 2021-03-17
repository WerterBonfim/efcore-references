using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CursoEFCore.Data.Configuration
{

    public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedidos");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.IniciadoEm).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
            builder.Property(p => p.Status).HasConversion<string>();
            builder.Property(p => p.TipoFrete).HasConversion<int>();
            builder.Property(p => p.Observacao).HasColumnType("varchar(512)");


            builder.HasMany(p => p.Itens)
                .WithOne(p => p.Pedido)
                // Os itens devem ser deletados primeiros
                //.OnDelete(DeleteBehavior.Restrict)
                // Deleta todos os itens do pedido
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
