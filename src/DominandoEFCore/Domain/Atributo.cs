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
}