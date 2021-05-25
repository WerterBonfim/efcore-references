using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DominandoEFCore.Domain
{
    public class Livro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        [MaxLength(15, ErrorMessage = "Quantidade maxima de caracteres excedida")]
        public string Autor { get; set; }
    }
}