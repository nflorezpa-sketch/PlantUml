using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using ApplicationCore.Domain.CEN;
using ApplicationCore.Domain.Enums;

namespace ApplicationCore.Domain.CP;

public class ConfirmarCompraCP
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly ITransaccionRepository _transaccionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmarCompraCP(
        IPedidoRepository pedidoRepository,
        ITransaccionRepository transaccionRepository,
        IUnitOfWork unitOfWork)
    {
        _pedidoRepository = pedidoRepository;
        _transaccionRepository = transaccionRepository;
        _unitOfWork = unitOfWork;
    }

    public long Ejecutar(long pedidoId, string metodoPago)
    {
        _unitOfWork.BeginTransaction();
        
        try
        {
            var pedido = _pedidoRepository.GetById(pedidoId);
            if (pedido == null)
                throw new Exception($"Pedido con Id {pedidoId} no encontrado");

            var transaccion = new Transaccion
            {
                Fecha = DateTime.Now,
                Total = pedido.Total,
                MetodoPago = metodoPago,
                TipoOperacion = TipoOperacion.Cobro,
                Usuario = pedido.Usuario,
                Pedido = pedido
            };

            _transaccionRepository.Add(transaccion);
            pedido.Transaccion = transaccion;
            _pedidoRepository.Update(pedido);

            _unitOfWork.Commit();
            return transaccion.Id;
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
}
