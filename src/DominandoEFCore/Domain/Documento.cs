using System;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore.Domain
{
    public class Documento
    {
        private string _cpf;

        public int Id { get; set; }

        public void DefinirCpf(string cpf)
        {
            // Validações
            if (string.IsNullOrEmpty(cpf))
                throw new Exception("CPF Inválido");

            _cpf = cpf;
        }

        [BackingField(nameof(_cpf))]
        public string CPF => _cpf;
        //public string GetCpf() => _cpf;
    }
}