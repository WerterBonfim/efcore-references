using System.Threading.Tasks;
using DominandoEFCore.RepositoryPattern.Domain;

namespace DominandoEFCore.RepositoryPattern.Data.Repositories
{
    public interface IDepartamentoRepository
    {
        Task<Departamento> GetByIdAsync(int id);
        void Add(Departamento departamento);
        bool Save();
    }
}