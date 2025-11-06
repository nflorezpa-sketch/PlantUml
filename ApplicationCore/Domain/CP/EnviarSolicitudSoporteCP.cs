using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using ApplicationCore.Domain.Enums;

namespace ApplicationCore.Domain.CP;

/// <summary>
/// Caso de uso: Enviar solicitud de soporte
/// Permite a un usuario crear una solicitud de soporte técnico
/// </summary>
public class EnviarSolicitudSoporteCP
{
    private readonly ISoporteRepository _soporteRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EnviarSolicitudSoporteCP(
        ISoporteRepository soporteRepository,
        IUsuarioRepository usuarioRepository,
        IUnitOfWork unitOfWork)
    {
        _soporteRepository = soporteRepository;
        _usuarioRepository = usuarioRepository;
        _unitOfWork = unitOfWork;
    }

    public long Ejecutar(long usuarioId, string descripcion)
    {
        _unitOfWork.BeginTransaction();
        
        try
        {
            // Validar usuario
            var usuario = _usuarioRepository.GetById(usuarioId);
            if (usuario == null)
                throw new Exception($"Usuario con Id {usuarioId} no encontrado");

            // Validar descripción
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ArgumentException("Debe proporcionar una descripción del problema");

            if (descripcion.Length < 10)
                throw new ArgumentException("La descripción debe tener al menos 10 caracteres");

            // Crear solicitud de soporte
            var soporte = new Soporte
            {
                Descripcion = descripcion.Trim(),
                Estado = EstadoSoporte.SinSolucionar,
                Usuario = usuario
            };

            _soporteRepository.Add(soporte);
            _unitOfWork.Commit();
            
            return soporte.Id;
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
}
