using System;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore.DicasTruques.Domain
{
    [Keyless]
    public class UsuarioFuncao
    {
        public Guid UsuarioId { get; set; }
        public Guid FuncaoId { get; set; }
    }
}