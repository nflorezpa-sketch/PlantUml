using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;

namespace Infrastructure.Repositories;

public class TransaccionRepository : ITransaccionRepository
{
    private readonly ISession _session;

    public TransaccionRepository(ISession session)
    {
        _session = session;
    }

    public Transaccion? GetById(long id)
    {
        return _session.Get<Transaccion>(id);
    }

    public IEnumerable<Transaccion> GetAll()
    {
        return _session.Query<Transaccion>().ToList();
    }

    public void Add(Transaccion entity)
    {
        _session.Save(entity);
    }

    public void Update(Transaccion entity)
    {
        _session.Update(entity);
    }

    public void Delete(Transaccion entity)
    {
        _session.Delete(entity);
    }
}
