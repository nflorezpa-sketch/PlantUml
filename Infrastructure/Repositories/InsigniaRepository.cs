using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;

namespace Infrastructure.Repositories;

public class InsigniaRepository : IInsigniaRepository
{
    private readonly ISession _session;

    public InsigniaRepository(ISession session)
    {
        _session = session;
    }

    public Insignia? GetById(long id)
    {
        return _session.Get<Insignia>(id);
    }

    public IEnumerable<Insignia> GetAll()
    {
        return _session.Query<Insignia>().ToList();
    }

    public void Add(Insignia entity)
    {
        _session.Save(entity);
    }

    public void Update(Insignia entity)
    {
        _session.Update(entity);
    }

    public void Delete(Insignia entity)
    {
        _session.Delete(entity);
    }
}
