using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Meu.NamespaceTeste
{
    [Index(nameof(Id), Name = "Produtos_Id_uindex", IsUnique = true)]
    public partial class Produto
    {
        [Key]
        public int Id { get; set; }
        [StringLength(60)]
        public string Nome { get; set; }
        [Column(TypeName = "decimal(6, 2)")]
        public decimal? Preco { get; set; }
        [StringLength(200)]
        public string Descricao { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DataHoraCadastro { get; set; }
    }
}
