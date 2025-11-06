using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;

namespace Infrastructure.Repositories;

public class VideojuegoRepository : IVideojuegoRepository
{
    private readonly ISession _session;

    public VideojuegoRepository(ISession session)
    {
        _session = session;
    }

    public Videojuego? GetById(long id)
    {
        return _session.Get<Videojuego>(id);
    }

    public IEnumerable<Videojuego> GetAll()
    {
        return _session.Query<Videojuego>().ToList();
    }

    public void Add(Videojuego entity)
    {
        _session.Save(entity);
    }

    public void Update(Videojuego entity)
    {
        _session.Update(entity);
    }

    public void Delete(Videojuego entity)
    {
        _session.Delete(entity);
    }
    public IEnumerable<Videojuego> ReadFilterByCategoria(string categoria)
    {
        if (string.IsNullOrWhiteSpace(categoria))
            return Enumerable.Empty<Videojuego>();
            
        if (!Enum.TryParse(typeof(Categoria), categoria, true, out var parsed))
            return Enumerable.Empty<Videojuego>();

        var enumValue = (Categoria)parsed;

        return _session.Query<Videojuego>()
                    .Where(v => v.Categoria == enumValue)
                    .ToList();
    }
}
