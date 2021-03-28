using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DominandoEFCore.Domain
{
    public class Departamento
    {
        //private ILazyLoader _lazyLoader;
        private Action<object, string> _lazyLoader;
        
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        //public List<Funcionario> Funcionarios { get; set; }

        private List<Funcionario> _funcionarios;

        public List<Funcionario> Funcionarios
        {
            get
            {
                _lazyLoader?.Invoke(this, nameof(Funcionarios));
                return _funcionarios;
            }
            set => _funcionarios = value;

        }
        
        

        // EFCore
        public Departamento()
        {
            
        }

        private Departamento(Action<object, string> lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }
    }
}
