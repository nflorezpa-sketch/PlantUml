using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using ApplicationCore.Domain.Enums;

namespace ApplicationCore.Domain.CEN;

public class TransaccionCEN
{
    private readonly ITransaccionRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public TransaccionCEN(ITransaccionRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    // ============== CRUD OPERATIONS ==============

    /// <summary>
    /// New - Crea una nueva transacción
    /// </summary>
    public long New(DateTime fecha, decimal total, string metodoPago, TipoOperacion tipoOperacion, Usuario usuario)
    {
        if (total <= 0)
            throw new ArgumentException("El total debe ser mayor que 0.");
        if (string.IsNullOrWhiteSpace(metodoPago))
            throw new ArgumentException("El método de pago no puede estar vacío.");
        if (usuario == null)
            throw new ArgumentException("La transacción debe estar asociada a un usuario válido.");

        var transaccion = new Transaccion
        {
            Fecha = fecha,
            Total = total,
            MetodoPago = metodoPago,
            TipoOperacion = tipoOperacion,
            Usuario = usuario
        };

        _repository.Add(transaccion);
        _unitOfWork.SaveChanges();
        return transaccion.Id;
    }

    /// <summary>
    /// Modify - Modifica una transacción existente
    /// </summary>
    public void Modify(long id, DateTime fecha, decimal total, string metodoPago, TipoOperacion tipoOperacion)
    {
        var transaccion = _repository.GetById(id);
        if (transaccion == null)
            throw new Exception($"Transaccion con Id {id} no encontrada");

        if (total <= 0)
            throw new ArgumentException("El total debe ser positivo.");
        if (fecha > DateTime.Now)
            throw new ArgumentException("La fecha no puede ser futura.");

        transaccion.Fecha = fecha;
        transaccion.Total = total;
        transaccion.MetodoPago = metodoPago;
        transaccion.TipoOperacion = tipoOperacion;

        _repository.Update(transaccion);
        _unitOfWork.SaveChanges();
    }

    /// <summary>
    /// Destroy - Elimina una transacción
    /// </summary>
    public void Destroy(long id)
    {
        var transaccion = _repository.GetById(id);
        if (transaccion == null)
            throw new Exception($"Transaccion con Id {id} no encontrada");

        if (transaccion.Total > 10000)
            throw new InvalidOperationException("No se pueden eliminar transacciones mayores a 10000.");

        _repository.Delete(transaccion);
        _unitOfWork.SaveChanges();
    }

    /// <summary>
    /// ReadOID - Obtiene una transacción por su ID
    /// </summary>
    public Transaccion? ReadOID(long id) => _repository.GetById(id);

    /// <summary>
    /// ReadAll - Obtiene todas las transacciones
    /// </summary>
    public IEnumerable<Transaccion> ReadAll() => _repository.GetAll();

    // ============== READ FILTERS ==============

    /// <summary>
    /// ReadFilterByMetodoPago - Obtiene transacciones filtradas por método de pago
    /// </summary>
    public IEnumerable<Transaccion> ReadFilterByMetodoPago(string metodoPago)
    {
        if (string.IsNullOrWhiteSpace(metodoPago))
            throw new ArgumentException("Debe especificar un método de pago.");

        return _repository.GetAll().Where(t => t.MetodoPago.Equals(metodoPago, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// ReadFilterByTipoOperacion - Obtiene transacciones filtradas por tipo de operación
    /// </summary>
    public IEnumerable<Transaccion> ReadFilterByTipoOperacion(TipoOperacion tipoOperacion)
    {
        return _repository.GetAll().Where(t => t.TipoOperacion == tipoOperacion);
    }
}
