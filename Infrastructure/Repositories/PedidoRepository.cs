using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;

namespace Infrastructure.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly ISession _session;

    public PedidoRepository(ISession session)
    {
        _session = session;
    }

    public Pedido? GetById(long id)
    {
        return _session.Get<Pedido>(id);
    }

    public IEnumerable<Pedido> GetAll()
    {
        return _session.Query<Pedido>().ToList();
    }

    public void Add(Pedido entity)
    {
        _session.Save(entity);
    }

    public void Update(Pedido entity)
    {
        _session.Update(entity);
    }

    public void Delete(Pedido entity)
    {
        _session.Delete(entity);
    }
}
