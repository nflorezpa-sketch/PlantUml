using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using ApplicationCore.Domain.CEN;

namespace ApplicationCore.Domain.CP;

/// <summary>
/// Caso de uso: Publicar un videojuego por parte de un vendedor
/// Crea el videojuego y lo asocia al vendedor y a una categoría
/// </summary>
public class PublicarVideojuegoCP
{
    private readonly IVideojuegoRepository _videojuegoRepository;
    private readonly IVendedorRepository _vendedorRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PublicarVideojuegoCP(
        IVideojuegoRepository videojuegoRepository,
        IVendedorRepository vendedorRepository,
        ICategoriaRepository categoriaRepository,
        IUnitOfWork unitOfWork)
    {
        _videojuegoRepository = videojuegoRepository;
        _vendedorRepository = vendedorRepository;
        _categoriaRepository = categoriaRepository;
        _unitOfWork = unitOfWork;
    }

    public long Ejecutar(long vendedorId, long categoriaId, float precio)
    {
        _unitOfWork.BeginTransaction();
        
        try
        {
            // Validar que el vendedor existe
            var vendedor = _vendedorRepository.GetById(vendedorId);
            if (vendedor == null)
                throw new Exception($"Vendedor con Id {vendedorId} no encontrado");

            // Validar que la categoría existe
            var categoria = _categoriaRepository.GetById(categoriaId);
            if (categoria == null)
                throw new Exception($"Categoría con Id {categoriaId} no encontrada");

            // Validar precio
            if (precio <= 0)
                throw new ArgumentException("El precio debe ser mayor que cero");

            // Crear el videojuego
            var videojuego = new Videojuego
            {
                Precio = precio,
                Categoria = categoria
            };

            _videojuegoRepository.Add(videojuego);

            // Asociar el videojuego al vendedor
            vendedor.VideojuegosPublicados.Add(videojuego);
            _vendedorRepository.Update(vendedor);

            _unitOfWork.Commit();
            return videojuego.Id;
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
}
