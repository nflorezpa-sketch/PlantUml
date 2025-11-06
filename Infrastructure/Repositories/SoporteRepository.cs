using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;

namespace Infrastructure.Repositories;

public class SoporteRepository : ISoporteRepository
{
    private readonly ISession _session;

    public SoporteRepository(ISession session)
    {
        _session = session;
    }

    public Soporte? GetById(long id)
    {
        return _session.Get<Soporte>(id);
    }

    public IEnumerable<Soporte> GetAll()
    {
        return _session.Query<Soporte>().ToList();
    }

    public void Add(Soporte entity)
    {
        _session.Save(entity);
    }

    public void Update(Soporte entity)
    {
        _session.Update(entity);
    }

    public void Delete(Soporte entity)
    {
        _session.Delete(entity);
    }
}
