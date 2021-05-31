using DominandoEFCore.RepositoryPattern.Data.Repositories;

namespace DominandoEFCore.RepositoryPattern.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;

        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public bool Commit()
        {
            return _context.SaveChanges() > 0;
        }

        private IDepartamentoRepository _departamentoRepository;

        public IDepartamentoRepository DepartamentoRepository => 
            _departamentoRepository ?? (_departamentoRepository = new DepartamentoRepository(_context));
    }
}