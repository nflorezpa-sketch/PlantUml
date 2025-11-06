using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using ApplicationCore.Domain.Enums;

namespace ApplicationCore.Domain.CEN;

public class SoporteCEN
{
    private readonly ISoporteRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public SoporteCEN(ISoporteRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    // ============== CRUD OPERATIONS ==============

    /// <summary>
    /// New - Crea una nueva solicitud de soporte
    /// </summary>
    public long New(string descripcion, EstadoSoporte estado)
    {
        if (string.IsNullOrWhiteSpace(descripcion))
            throw new ArgumentException("La descripción no puede estar vacía.");

        var soporte = new Soporte
        {
            Descripcion = descripcion.Trim(),
            Estado = estado
        };

        _repository.Add(soporte);
        _unitOfWork.SaveChanges();
        return soporte.Id;
    }

    /// <summary>
    /// Modify - Modifica una solicitud de soporte existente
    /// </summary>
    public void Modify(long id, string descripcion, EstadoSoporte estado)
    {
        var soporte = _repository.GetById(id);
        if (soporte == null)
            throw new Exception($"Soporte con Id {id} no encontrado");

        if (estado == EstadoSoporte.Solucionado)
            throw new InvalidOperationException("No se puede modificar un problema solucionado.");
        if (string.IsNullOrWhiteSpace(descripcion))
            throw new ArgumentException("La descripción no puede estar vacía.");

        soporte.Descripcion = descripcion.Trim();
        soporte.Estado = estado;

        _repository.Update(soporte);
        _unitOfWork.SaveChanges();
    }

    /// <summary>
    /// Destroy - Elimina una solicitud de soporte
    /// </summary>
    public void Destroy(long id)
    {
        var soporte = _repository.GetById(id);
        if (soporte == null)
            throw new Exception($"Soporte con Id {id} no encontrado");

        if (soporte.Estado != EstadoSoporte.Solucionado)
            throw new InvalidOperationException("Solo se pueden eliminar soportes solucionados.");

        _repository.Delete(soporte);
        _unitOfWork.SaveChanges();
    }

    /// <summary>
    /// ReadOID - Obtiene una solicitud de soporte por su ID
    /// </summary>
    public Soporte? ReadOID(long id)
    {
        return _repository.GetById(id);
    }

    /// <summary>
    /// ReadAll - Obtiene todas las solicitudes de soporte
    /// </summary>
    public IEnumerable<Soporte> ReadAll()
    {
        return _repository.GetAll();
    }

    // ============== READ FILTERS ==============

    /// <summary>
    /// ReadFilterByEstado - Obtiene solicitudes de soporte filtradas por estado
    /// </summary>
    public IEnumerable<Soporte> ReadFilterByEstado(EstadoSoporte estado)
    {
        return _repository.GetAll().Where(s => s.Estado == estado);
    }
}
