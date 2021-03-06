using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore.Domain
{
    [Table("TabelaAtributos")]
    [Index(nameof(Descricao), nameof(Id), IsUnique = true)]
    [Comment("Meu comentario sobre minha tabela")]
    public class Atributo
    {
        [Key] 
        public int Id { get; set; }

        [Column("MinhaDescrição", TypeName = "VARCHAR(100)")]
        [Comment("Meu comentario sobre essa coluna")]
        public string Descricao { get; set; }
        
        //[Required]
        // O EF não irar incluir essa propriedade nas instruções de insert
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(255)]
        public string Observacao { get; set; }
        
        
    }

    public class Aeroporto
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        [NotMapped]
        public string PropriedadeTeste { get; set; }

        [InverseProperty("AeroportoPartida")]
        public ICollection<Voo> VoosDePartida { get; set; }
        
        [InverseProperty("AeroportoChegada")]
        public ICollection<Voo> VoosDeChegada { get; set; }
    }

    [Keyless]
    public class RelatorioFinanceiro
    {
        public string Descricao { get; set; }
        public decimal Total { get; set; }
        public DateTime Data { get; set; }
    } 

    [NotMapped]
    public class Voo
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public Aeroporto AeroportoPartida { get; set; }
        public Aeroporto AeroportoChegada { get; set; }
    }
}