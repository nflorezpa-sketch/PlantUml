using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using ApplicationCore.Domain.CEN;

namespace ApplicationCore.Domain.CP;

/// <summary>
/// Caso de uso: Comprar videojuegos
/// Crea un pedido con múltiples videojuegos y calcula el total
/// </summary>
public class ComprarVideojuegosCP
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IVideojuegoRepository _videojuegoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ComprarVideojuegosCP(
        IPedidoRepository pedidoRepository,
        IUsuarioRepository usuarioRepository,
        IVideojuegoRepository videojuegoRepository,
        IUnitOfWork unitOfWork)
    {
        _pedidoRepository = pedidoRepository;
        _usuarioRepository = usuarioRepository;
        _videojuegoRepository = videojuegoRepository;
        _unitOfWork = unitOfWork;
    }

    public long Ejecutar(long usuarioId, List<long> videojuegoIds)
    {
        _unitOfWork.BeginTransaction();
        
        try
        {
            // Validar usuario
            var usuario = _usuarioRepository.GetById(usuarioId);
            if (usuario == null)
                throw new Exception($"Usuario con Id {usuarioId} no encontrado");

            // Validar que hay videojuegos
            if (videojuegoIds == null || !videojuegoIds.Any())
                throw new ArgumentException("Debe seleccionar al menos un videojuego");

            // Obtener los videojuegos
            var videojuegos = new List<Videojuego>();
            decimal total = 0;

            foreach (var videoId in videojuegoIds)
            {
                var videojuego = _videojuegoRepository.GetById(videoId);
                if (videojuego == null)
                    throw new Exception($"Videojuego con Id {videoId} no encontrado");

                videojuegos.Add(videojuego);
                total += (decimal)videojuego.Precio;
            }

            // Crear el pedido
            var pedido = new Pedido
            {
                Usuario = usuario,
                Total = total
            };

            // Agregar videojuegos al pedido
            foreach (var videojuego in videojuegos)
            {
                pedido.Videojuegos.Add(videojuego);
            }

            _pedidoRepository.Add(pedido);

            // Agregar videojuegos al usuario
            foreach (var videojuego in videojuegos)
            {
                usuario.Videojuegos.Add(videojuego);
            }
            _usuarioRepository.Update(usuario);

            _unitOfWork.Commit();
            return pedido.Id;
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
}
