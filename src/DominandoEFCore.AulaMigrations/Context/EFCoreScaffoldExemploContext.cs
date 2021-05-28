using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Meu.NamespaceTeste;

#nullable disable

namespace Meu.NamespaceTeste.Contexto
{
    public partial class EFCoreScaffoldExemploContext : DbContext
    {
        public EFCoreScaffoldExemploContext()
        {
        }

        public EFCoreScaffoldExemploContext(DbContextOptions<EFCoreScaffoldExemploContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Produto> Produtos { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost, 1433;Database=EFCoreScaffoldExemplo;User Id=sa;Password=!123Senha;Application Name=\"Rider CursoEFCore\";pooling=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Produto>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("Produtos_pk")
                    .IsClustered(false);

                entity.HasComment("Comentario qualquer para ver como o EF se comporta");

                entity.Property(e => e.Descricao).IsUnicode(false);

                entity.Property(e => e.Nome).IsUnicode(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("Usuarios_pk")
                    .IsClustered(false);

                entity.HasComment("Comentario da tabela de usuarios");

                entity.Property(e => e.CPF).IsUnicode(false);

                entity.Property(e => e.Nome).IsUnicode(false);

                entity.Property(e => e.email).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
