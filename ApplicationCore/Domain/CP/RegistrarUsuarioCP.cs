using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using ApplicationCore.Domain.CEN;

namespace ApplicationCore.Domain.CP;

/// <summary>
/// Caso de uso: Registrar un nuevo usuario en el sistema
/// Valida datos y crea el usuario con sus credenciales
/// </summary>
public class RegistrarUsuarioCP
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegistrarUsuarioCP(
        IUsuarioRepository usuarioRepository,
        IUnitOfWork unitOfWork)
    {
        _usuarioRepository = usuarioRepository;
        _unitOfWork = unitOfWork;
    }

    public long Ejecutar(string nombreUsuario, string correo, string telefono, string apodo, string contraseña)
    {
        _unitOfWork.BeginTransaction();
        
        try
        {
            // Validar que el correo no esté duplicado
            var usuarioExistente = _usuarioRepository.GetAll()
                .FirstOrDefault(u => u.Correo.ToLower() == correo.ToLower());
            
            if (usuarioExistente != null)
                throw new InvalidOperationException($"Ya existe un usuario con el correo {correo}");

            // Validar nombre de usuario único
            var nombreExistente = _usuarioRepository.GetAll()
                .FirstOrDefault(u => u.NombreUsuario.ToLower() == nombreUsuario.ToLower());
            
            if (nombreExistente != null)
                throw new InvalidOperationException($"El nombre de usuario {nombreUsuario} ya está en uso");

            // Validar formato de contraseña
            if (contraseña.Length < 6)
                throw new ArgumentException("La contraseña debe tener al menos 6 caracteres");

            // Crear nuevo usuario
            var nuevoUsuario = new Usuario
            {
                NombreUsuario = nombreUsuario.Trim(),
                Correo = correo.Trim().ToLower(),
                Telefono = telefono,
                Apodo = apodo,
                Contraseña = contraseña // En producción debería hashearse
            };

            _usuarioRepository.Add(nuevoUsuario);
            _unitOfWork.Commit();
            
            return nuevoUsuario.Id;
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
}
