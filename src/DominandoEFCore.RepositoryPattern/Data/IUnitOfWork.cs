using System;
using DominandoEFCore.RepositoryPattern.Data.Repositories;

namespace DominandoEFCore.RepositoryPattern.Data
{
    public interface IUnitOfWork : IDisposable
    {
        bool Commit();
        IDepartamentoRepository DepartamentoRepository { get; }
    }
}