using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;

namespace Infrastructure.Repositories;

public class ReporteRepository : IReporteRepository
{
    private readonly ISession _session;

    public ReporteRepository(ISession session)
    {
        _session = session;
    }

    public Reporte? GetById(long id)
    {
        return _session.Get<Reporte>(id);
    }

    public IEnumerable<Reporte> GetAll()
    {
        return _session.Query<Reporte>().ToList();
    }

    public void Add(Reporte entity)
    {
        _session.Save(entity);
    }

    public void Update(Reporte entity)
    {
        _session.Update(entity);
    }

    public void Delete(Reporte entity)
    {
        _session.Delete(entity);
    }
}
