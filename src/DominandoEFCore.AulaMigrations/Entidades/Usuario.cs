using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Meu.NamespaceTeste
{
    [Index(nameof(Id), Name = "Usuarios_Id_uindex", IsUnique = true)]
    public partial class Usuario
    {
        [Key]
        public int Id { get; set; }
        [StringLength(30)]
        public string Nome { get; set; }
        [Required]
        [StringLength(12)]
        public string CPF { get; set; }
        [Required]
        [StringLength(20)]
        public string email { get; set; }
    }
}
