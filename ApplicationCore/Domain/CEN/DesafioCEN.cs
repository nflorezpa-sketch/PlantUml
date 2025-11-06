using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;

namespace ApplicationCore.Domain.CEN;

public class DesafioCEN
{
    private readonly IDesafioRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DesafioCEN(IDesafioRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    // ============== CRUD OPERATIONS ==============

    /// <summary>
    /// New - Crea un nuevo desafío
    /// </summary>
    public long New(string nombre, string descripcion)
    {
        var desafio = new Desafio
        {
            Nombre = nombre,
            Descripcion = descripcion
        };
        _repository.Add(desafio);
        _unitOfWork.SaveChanges();
        return desafio.Id;
    }

    /// <summary>
    /// Modify - Modifica un desafío existente
    /// </summary>
    public void Modify(long id, string nombre, string descripcion)
    {
        var desafio = _repository.GetById(id);
        if (desafio == null) throw new Exception($"Desafio con Id {id} no encontrado");
        
        desafio.Nombre = nombre;
        desafio.Descripcion = descripcion;
        
        _repository.Update(desafio);
        _unitOfWork.SaveChanges();
    }

    /// <summary>
    /// Destroy - Elimina un desafío
    /// </summary>
    public void Destroy(long id)
    {
        var desafio = _repository.GetById(id);
        if (desafio == null) throw new Exception($"Desafio con Id {id} no encontrado");
        
        _repository.Delete(desafio);
        _unitOfWork.SaveChanges();
    }

    /// <summary>
    /// ReadOID - Obtiene un desafío por su ID
    /// </summary>
    public Desafio? ReadOID(long id)
    {
        return _repository.GetById(id);
    }

    /// <summary>
    /// ReadAll - Obtiene todos los desafíos
    /// </summary>
    public IEnumerable<Desafio> ReadAll()
    {
        return _repository.GetAll();
    }
}
