using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;

namespace Infrastructure.Repositories;

public class VendedorRepository : IVendedorRepository
{
    private readonly ISession _session;

    public VendedorRepository(ISession session)
    {
        _session = session;
    }

    public Vendedor? GetById(long id)
    {
        return _session.Get<Vendedor>(id);
    }

    public IEnumerable<Vendedor> GetAll()
    {
        return _session.Query<Vendedor>().ToList();
    }

    public void Add(Vendedor entity)
    {
        _session.Save(entity);
    }

    public void Update(Vendedor entity)
    {
        _session.Update(entity);
    }

    public void Delete(Vendedor entity)
    {
        _session.Delete(entity);
    }
}
