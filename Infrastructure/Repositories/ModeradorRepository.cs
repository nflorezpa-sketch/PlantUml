using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;

namespace Infrastructure.Repositories;

public class ModeradorRepository : IModeradorRepository
{
    private readonly ISession _session;

    public ModeradorRepository(ISession session)
    {
        _session = session;
    }

    public Moderador? GetById(long id)
    {
        return _session.Get<Moderador>(id);
    }

    public IEnumerable<Moderador> GetAll()
    {
        return _session.Query<Moderador>().ToList();
    }

    public void Add(Moderador entity)
    {
        _session.Save(entity);
    }

    public void Update(Moderador entity)
    {
        _session.Update(entity);
    }

    public void Delete(Moderador entity)
    {
        _session.Delete(entity);
    }
}
