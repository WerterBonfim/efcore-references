using System.Threading.Tasks;
using DominandoEFCore.RepositoryPattern.Domain;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore.RepositoryPattern.Data.Repositories
{
    public class DepartamentoRepository : IDepartamentoRepository
    {
        private readonly ApplicationContext _context;

        public DepartamentoRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Departamento> GetByIdAsync(int id)
        {
            return await _context
                .Departamentos
                .Include(x => x.Colaboradores)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Add(Departamento departamento)
        {
            _context.Departamentos.Add(departamento);
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}