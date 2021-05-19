using System;

namespace DominandoEFCore.Domain
{
    public class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public override string ToString()
        {
            return $"Id:{Id} Nome: {Nome}";
        }
    }

    public class Instrutor : Pessoa
    {
        public DateTime Desde { get; set; }
        public string Tecnologia { get; set; }

        public override string ToString()
        {
            return base.ToString() + $" Tecnologia: {Tecnologia} Desde: {Desde.ToString()}";
        }
    }

    public class Aluno : Pessoa
    {
        public int Idade { get; set; }
        public DateTime DataContrato { get; set; }

        public override string ToString()
        {
            return base.ToString() + $"Idade: {Idade} Data contrato: {DataContrato.ToString()}";
        }
    }
}