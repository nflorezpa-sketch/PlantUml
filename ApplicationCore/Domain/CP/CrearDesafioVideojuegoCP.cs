using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;

namespace ApplicationCore.Domain.CP;

/// <summary>
/// Caso de uso: Crear un desafío asociado a videojuegos
/// Crea desafíos que los jugadores pueden completar en videojuegos específicos
/// </summary>
public class CrearDesafioVideojuegoCP
{
    private readonly IDesafioRepository _desafioRepository;
    private readonly IVideojuegoRepository _videojuegoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CrearDesafioVideojuegoCP(
        IDesafioRepository desafioRepository,
        IVideojuegoRepository videojuegoRepository,
        IUnitOfWork unitOfWork)
    {
        _desafioRepository = desafioRepository;
        _videojuegoRepository = videojuegoRepository;
        _unitOfWork = unitOfWork;
    }

    public long Ejecutar(string nombre, string descripcion, List<long> videojuegoIds)
    {
        _unitOfWork.BeginTransaction();
        
        try
        {
            // Validar datos del desafío
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("Debe proporcionar un nombre para el desafío");

            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ArgumentException("Debe proporcionar una descripción para el desafío");

            if (videojuegoIds == null || !videojuegoIds.Any())
                throw new ArgumentException("Debe asociar el desafío a al menos un videojuego");

            // Crear el desafío
            var desafio = new Desafio
            {
                Nombre = nombre.Trim(),
                Descripcion = descripcion.Trim()
            };

            // Asociar videojuegos al desafío
            foreach (var videoId in videojuegoIds)
            {
                var videojuego = _videojuegoRepository.GetById(videoId);
                if (videojuego == null)
                    throw new Exception($"Videojuego con Id {videoId} no encontrado");

                desafio.Videojuegos.Add(videojuego);
            }

            _desafioRepository.Add(desafio);
            _unitOfWork.Commit();
            
            return desafio.Id;
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
}
