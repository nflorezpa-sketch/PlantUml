using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;

namespace ApplicationCore.Domain.CEN;

public class PedidoCEN
{
    private readonly IPedidoRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public PedidoCEN(IPedidoRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    // ============== CRUD OPERATIONS ==============

    /// <summary>
    /// New - Crea un nuevo pedido
    /// </summary>
    public long New(decimal total, Usuario usuario)
    {
        if (total < 0)
            throw new ArgumentException("El total no puede ser negativo");
        if (usuario == null)
            throw new ArgumentException("El usuario es obligatorio");

        var pedido = new Pedido
        {
            Total = total,
            Usuario = usuario
        };
        _repository.Add(pedido);
        _unitOfWork.SaveChanges();
        return pedido.Id;
    }

    /// <summary>
    /// Modify - Modifica un pedido existente
    /// </summary>
    public void Modify(long id, decimal total)
    {
        var pedido = _repository.GetById(id);
        if (pedido == null) throw new Exception($"Pedido con Id {id} no encontrado");
        
        if (total < 0)
            throw new ArgumentException("El total no puede ser negativo");

        pedido.Total = total;
        
        _repository.Update(pedido);
        _unitOfWork.SaveChanges();
    }

    /// <summary>
    /// Destroy - Elimina un pedido
    /// </summary>
    public void Destroy(long id)
    {
        var pedido = _repository.GetById(id);
        if (pedido == null) throw new Exception($"Pedido con Id {id} no encontrado");
        
        _repository.Delete(pedido);
        _unitOfWork.SaveChanges();
    }

    /// <summary>
    /// ReadOID - Obtiene un pedido por su ID
    /// </summary>
    public Pedido? ReadOID(long id) => _repository.GetById(id);

    /// <summary>
    /// ReadAll - Obtiene todos los pedidos
    /// </summary>
    public IEnumerable<Pedido> ReadAll() => _repository.GetAll();
}
