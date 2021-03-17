using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CursoEFCore.Domain {


    //[Table("TB_Cliente")]
    public class Cliente {
        //[Key]
        public int Id { get; set; } 
        //[Required]
        public string Nome { get; set; }
        //[Column("Fone")]
        public string Telefone { get; set; }
        public string CEP { get; set; }
        public string Estado { get; set; }
        public string  Cidade { get; set; }
    }
}
