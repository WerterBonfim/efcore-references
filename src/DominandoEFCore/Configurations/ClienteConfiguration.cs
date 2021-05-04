using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DominandoEFCore.Configurations
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.OwnsOne(x => x.Endereco, end =>
            {
                end
                    .Property(p => p.Bairro)
                    .HasColumnName("Bairro");

                end.ToTable("Endereço");


            });
        }
    }
}