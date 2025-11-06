using ApplicationCore.Domain.Repositories;
using NHibernate;

namespace Infrastructure.UnitOfWork;

public class NHUnitOfWork : IUnitOfWork
{
    private readonly ISession _session;
    private ITransaction? _transaction;

    public NHUnitOfWork(ISession session)
    {
        _session = session;
    }

    public void SaveChanges()
    {
        _session.Flush();
    }

    public void BeginTransaction()
    {
        _transaction = _session.BeginTransaction();
    }

    public void Commit()
    {
        _transaction?.Commit();
        _transaction?.Dispose();
        _transaction = null;
    }

    public void Rollback()
    {
        _transaction?.Rollback();
        _transaction?.Dispose();
        _transaction = null;
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _session?.Dispose();
    }
}
