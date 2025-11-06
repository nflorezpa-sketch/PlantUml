using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;

namespace Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly ISession _session;

    public UsuarioRepository(ISession session)
    {
        _session = session;
    }

    public Usuario? GetById(long id)
    {
        return _session.Get<Usuario>(id);
    }

    public IEnumerable<Usuario> GetAll()
    {
        return _session.Query<Usuario>().ToList();
    }

    public void Add(Usuario entity)
    {
        _session.Save(entity);
    }

    public void Update(Usuario entity)
    {
        _session.Update(entity);
    }

    public void Delete(Usuario entity)
    {
        _session.Delete(entity);
    }

    public IEnumerable<Usuario> ReadFilterByNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return Enumerable.Empty<Usuario>();

        return _session.Query<Usuario>()
            .Where(u => u.NombreUsuario.ToLower().Contains(nombre.ToLower()))
            .ToList();
    }
}
