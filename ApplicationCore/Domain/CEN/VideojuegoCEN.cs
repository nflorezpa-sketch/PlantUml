using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;

namespace ApplicationCore.Domain.CEN;

public class VideojuegoCEN
{
    private readonly IVideojuegoRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public VideojuegoCEN(IVideojuegoRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    // ============== CRUD OPERATIONS ==============

    /// <summary>
    /// New - Crea un nuevo videojuego
    /// </summary>
    public long New(float precio)
    {
        if (precio <= 0)
            throw new ArgumentException("El precio debe ser mayor que 0.");

        var videojuego = new Videojuego
        {
            Precio = precio
        };

        _repository.Add(videojuego);
        _unitOfWork.SaveChanges();
        return videojuego.Id;
    }

    /// <summary>
    /// Modify - Modifica un videojuego existente
    /// </summary>
    public void Modify(long id, float? precio = null)
    {
        var videojuego = _repository.GetById(id);
        if (videojuego == null)
            throw new Exception($"Videojuego con Id {id} no encontrado");

        if (precio.HasValue && precio <= 0)
            throw new ArgumentException("El precio debe ser positivo.");

        if (precio.HasValue)
            videojuego.Precio = precio.Value;

        _repository.Update(videojuego);
        _unitOfWork.SaveChanges();
    }

    /// <summary>
    /// Destroy - Elimina un videojuego
    /// </summary>
    public void Destroy(long id)
    {
        var videojuego = _repository.GetById(id);
        if (videojuego == null)
            throw new Exception($"Videojuego con Id {id} no encontrado");

        if (videojuego.Precio > 200)
            throw new InvalidOperationException("No se pueden eliminar videojuegos con precio superior a 200.");

        _repository.Delete(videojuego);
        _unitOfWork.SaveChanges();
    }

    /// <summary>
    /// ReadOID - Obtiene un videojuego por su ID
    /// </summary>
    public Videojuego? ReadOID(long id) => _repository.GetById(id);

    /// <summary>
    /// ReadAll - Obtiene todos los videojuegos
    /// </summary>
    public IEnumerable<Videojuego> ReadAll() => _repository.GetAll();

    // ============== READ FILTERS ==============

    /// <summary>
    /// ReadFilterByCategoria - Obtiene videojuegos filtrados por categoría
    /// </summary>
    public IEnumerable<Videojuego> ReadFilterByCategoria(string categoria)
    {
        if (string.IsNullOrWhiteSpace(categoria))
            throw new ArgumentException("Debe especificar una categoría para el filtro.");

        return _repository.ReadFilterByCategoria(categoria);
    }

    /// <summary>
    /// ReadFilterByPrecio - Obtiene videojuegos filtrados por rango de precio
    /// </summary>
    public IEnumerable<Videojuego> ReadFilterByPrecio(float precioMin, float precioMax)
    {
        if (precioMin < 0 || precioMax < 0)
            throw new ArgumentException("Los precios no pueden ser negativos.");
        if (precioMin > precioMax)
            throw new ArgumentException("El precio mínimo no puede ser mayor que el precio máximo.");

        return _repository.GetAll().Where(v => v.Precio >= precioMin && v.Precio <= precioMax);
    }
}
