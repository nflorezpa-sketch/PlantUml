using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;

namespace ApplicationCore.Domain.CEN;

public class CategoriaCEN
{
    private readonly ICategoriaRepository _repository;
    private readonly IVideojuegoRepository _videojuegoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CategoriaCEN(ICategoriaRepository repository, IVideojuegoRepository videojuegoRepository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _videojuegoRepository = videojuegoRepository;
        _unitOfWork = unitOfWork;
    }

    // ============== CRUD OPERATIONS ==============

    /// <summary>
    /// New - Crea una nueva categoría
    /// </summary>
    public long New(string nombre, string descripcion)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre de la categoría es obligatorio");

        var categoria = new Categoria
        {
            Nombre = nombre.Trim(),
            Descripcion = descripcion
        };
        _repository.Add(categoria);
        _unitOfWork.SaveChanges();
        return categoria.Id;
    }

    /// <summary>
    /// Modify - Modifica una categoría existente
    /// </summary>
    public void Modify(long id, string nombre, string descripcion)
    {
        var categoria = _repository.GetById(id);
        if (categoria == null) throw new Exception($"Categoria con Id {id} no encontrado");
        
        categoria.Nombre = nombre.Trim();
        categoria.Descripcion = descripcion;
        
        _repository.Update(categoria);
        _unitOfWork.SaveChanges();
    }

    /// <summary>
    /// Destroy - Elimina una categoría
    /// </summary>
    public void Destroy(long id)
    {
        var categoria = _repository.GetById(id);
        if (categoria == null) throw new Exception($"Categoria con Id {id} no encontrado");
        
        _repository.Delete(categoria);
        _unitOfWork.SaveChanges();
    }

    /// <summary>
    /// ReadOID - Obtiene una categoría por su ID
    /// </summary>
    public Categoria? ReadOID(long id) => _repository.GetById(id);

    /// <summary>
    /// ReadAll - Obtiene todas las categorías
    /// </summary>
    public IEnumerable<Categoria> ReadAll() => _repository.GetAll();
}
