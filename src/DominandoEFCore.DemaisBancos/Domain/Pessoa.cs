using System;

namespace DominandoEFCore.DemaisBancos.Domain
{
    public class Pessoa
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
    }
}