using System.Collections;
using System.Collections.Generic;

namespace DominandoEFCore.Domain
{
    public class Estado
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public Governador Governador { get; set; }

        // Configuração de propriedade de navegação única:
        // Não a uma propriedade Estado na classe Cidade
        public ICollection<Cidade> Cidades { get; } = new List<Cidade>();
    }
}