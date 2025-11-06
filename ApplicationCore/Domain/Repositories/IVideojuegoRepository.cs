using ApplicationCore.Domain.EN;

namespace ApplicationCore.Domain.Repositories;

public interface IVideojuegoRepository : IRepository<Videojuego, long>
{
    IEnumerable<Videojuego> ReadFilterByCategoria(string categoria);
}
