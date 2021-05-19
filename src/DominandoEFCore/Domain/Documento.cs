using System;

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

        public string GetCpf() => _cpf;
    }
}