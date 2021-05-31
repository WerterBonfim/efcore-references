using System.Collections.Generic;
using DominandoEFCore.RepositoryPattern.Data.Repositories.Base;

namespace DominandoEFCore.RepositoryPattern.Domain
{
    public class Departamento : IRepository
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public List<Colaborador> Colaboradores { get; set; }
    }
}