using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using ApplicationCore.Domain.Enums;

namespace ApplicationCore.Domain.CP;

/// <summary>
/// Caso de uso: Asignar una insignia a un usuario
/// Valida y asigna insignias de perfil, marco, fondo o icono
/// </summary>
public class AsignarInsigniaCP
{
    private readonly IInsigniaRepository _insigniaRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AsignarInsigniaCP(
        IInsigniaRepository insigniaRepository,
        IUsuarioRepository usuarioRepository,
        IUnitOfWork unitOfWork)
    {
        _insigniaRepository = insigniaRepository;
        _usuarioRepository = usuarioRepository;
        _unitOfWork = unitOfWork;
    }

    public long Ejecutar(long usuarioId, TipoInsignia tipoInsignia, string rutaImagen)
    {
        _unitOfWork.BeginTransaction();
        
        try
        {
            // Validar usuario
            var usuario = _usuarioRepository.GetById(usuarioId);
            if (usuario == null)
                throw new Exception($"Usuario con Id {usuarioId} no encontrado");

            // Validar ruta de imagen
            if (string.IsNullOrWhiteSpace(rutaImagen))
                throw new ArgumentException("Debe proporcionar una ruta de imagen válida");

            // Crear la insignia
            var insignia = new Insignia
            {
                Perfil = tipoInsignia,
                RutaDelImg = rutaImagen.Trim(),
                Usuario = usuario
            };

            _insigniaRepository.Add(insignia);

            // Agregar insignia al usuario
            usuario.Insignias.Add(insignia);
            _usuarioRepository.Update(usuario);

            _unitOfWork.Commit();
            return insignia.Id;
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
}
