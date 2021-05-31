using System.Threading.Tasks;
using DominandoEFCore.RepositoryPattern.Data.Repositories.Base;
using DominandoEFCore.RepositoryPattern.Domain;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore.RepositoryPattern.Data.Repositories
{
    public class DepartamentoRepository : GenericRepository<Departamento>, IDepartamentoRepository
    { 
        public DepartamentoRepository(ApplicationContext context) : base(context)
        {
            
        }
    }
}