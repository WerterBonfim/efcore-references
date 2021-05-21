using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DominandoEFCore.Domain
{
    [Table("TabelaAtributos")]
    public class Atributo
    {
        [Key] 
        public int Id { get; set; }

        [Column("MinhaDescrição", TypeName = "VARCHAR(100)")]
        public string Descricao { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string Observacao { get; set; }
        
        
    }

    public class Aeroporto
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        [InverseProperty("AeroportoPartida")]
        public ICollection<Voo> VoosDePartida { get; set; }
        
        [InverseProperty("AeroportoChegada")]
        public ICollection<Voo> VoosDeChegada { get; set; }
    }

    public class Voo
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public Aeroporto AeroportoPartida { get; set; }
        public Aeroporto AeroportoChegada { get; set; }
    }
}