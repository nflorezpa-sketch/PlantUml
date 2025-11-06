using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;

namespace ApplicationCore.Domain.CP;

/// <summary>
/// Caso de uso: Filtrar videojuegos por rango de precio
/// Permite a los usuarios buscar videojuegos dentro de un presupuesto
/// </summary>
public class FiltrarVideojuegosPorPrecioCP
{
    private readonly IVideojuegoRepository _videojuegoRepository;

    public FiltrarVideojuegosPorPrecioCP(IVideojuegoRepository videojuegoRepository)
    {
        _videojuegoRepository = videojuegoRepository;
    }

    public IEnumerable<Videojuego> Ejecutar(float precioMinimo, float precioMaximo)
    {
        // Validar precios
        if (precioMinimo < 0)
            throw new ArgumentException("El precio mínimo no puede ser negativo");

        if (precioMaximo < precioMinimo)
            throw new ArgumentException("El precio máximo debe ser mayor o igual al precio mínimo");

        // Filtrar videojuegos por rango de precio
        return _videojuegoRepository.GetAll()
            .Where(v => v.Precio >= precioMinimo && v.Precio <= precioMaximo)
            .OrderBy(v => v.Precio)
            .ToList();
    }

    public IEnumerable<Videojuego> EjecutarPorCategoria(float precioMinimo, float precioMaximo, long categoriaId)
    {
        // Validar precios
        if (precioMinimo < 0)
            throw new ArgumentException("El precio mínimo no puede ser negativo");

        if (precioMaximo < precioMinimo)
            throw new ArgumentException("El precio máximo debe ser mayor o igual al precio mínimo");

        // Filtrar videojuegos por rango de precio y categoría
        return _videojuegoRepository.GetAll()
            .Where(v => v.Precio >= precioMinimo && 
                       v.Precio <= precioMaximo &&
                       v.Categoria != null && 
                       v.Categoria.Id == categoriaId)
            .OrderBy(v => v.Precio)
            .ToList();
    }
}
