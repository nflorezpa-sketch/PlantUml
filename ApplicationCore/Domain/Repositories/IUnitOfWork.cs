namespace ApplicationCore.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    void SaveChanges();
    void BeginTransaction();
    void Commit();
    void Rollback();
}
