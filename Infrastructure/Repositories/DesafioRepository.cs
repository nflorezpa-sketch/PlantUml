using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;

namespace Infrastructure.Repositories;

public class DesafioRepository : IDesafioRepository
{
    private readonly ISession _session;

    public DesafioRepository(ISession session)
    {
        _session = session;
    }

    public Desafio? GetById(long id)
    {
        return _session.Get<Desafio>(id);
    }

    public IEnumerable<Desafio> GetAll()
    {
        return _session.Query<Desafio>().ToList();
    }

    public void Add(Desafio entity)
    {
        _session.Save(entity);
    }

    public void Update(Desafio entity)
    {
        _session.Update(entity);
    }

    public void Delete(Desafio entity)
    {
        _session.Delete(entity);
    }
}
