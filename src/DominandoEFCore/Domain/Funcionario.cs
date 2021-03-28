using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DominandoEFCore.Domain
{
    public class Funcionario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public int DepartamentoId { get; set; }
        //public virtual Departamento Departamento { get; set; }
        
        
        public Departamento Departamento { get; set; }

        //EF
        public Funcionario() {
            
        }

        private ILazyLoader _lazyLoader;

        private Funcionario(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }
    }
}   