using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DominandoEFCore.Configurations
{
    public class PessoaConfiguration : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder
                .ToTable("Pessoas")
                .HasDiscriminator<int>("TipoPessoa")
                .HasValue<Pessoa>(1)
                .HasValue<Instrutor>(2)
                .HasValue<Aluno>(8);
        }
    }
}