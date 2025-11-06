using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;

namespace Infrastructure.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly ISession _session;

    public CategoriaRepository(ISession session)
    {
        _session = session;
    }

    public Categoria? GetById(long id)
    {
        return _session.Get<Categoria>(id);
    }

    public IEnumerable<Categoria> GetAll()
    {
        return _session.Query<Categoria>().ToList();
    }

    public void Add(Categoria entity)
    {
        _session.Save(entity);
    }

    public void Update(Categoria entity)
    {
        _session.Update(entity);
    }

    public void Delete(Categoria entity)
    {
        _session.Delete(entity);
    }
}
