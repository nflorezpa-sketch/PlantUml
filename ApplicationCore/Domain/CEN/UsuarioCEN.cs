using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;

namespace ApplicationCore.Domain.CEN;

public class UsuarioCEN
{
    private readonly IUsuarioRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UsuarioCEN(IUsuarioRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    // ============== CRUD OPERATIONS ==============
    
    /// <summary>
    /// New - Crea un nuevo usuario
    /// </summary>
    public long New(string nombreUsuario, string correo, string telefono, string apodo, string contraseña)
    {
        if (string.IsNullOrWhiteSpace(nombreUsuario))
            throw new ArgumentException("El nombre de usuario es obligatorio.");
        if (!correo.Contains("@"))
            throw new ArgumentException("El correo electrónico no es válido.");
        if (contraseña.Length < 6)
            throw new ArgumentException("La contraseña debe tener al menos 6 caracteres.");

        var usuario = new Usuario
        {
            NombreUsuario = nombreUsuario.Trim(),
            Correo = correo.Trim(),
            Telefono = telefono,
            Apodo = apodo,
            Contraseña = contraseña
        };

        _repository.Add(usuario);
        _unitOfWork.SaveChanges();
        return usuario.Id;
    }

    /// <summary>
    /// Modify - Modifica un usuario existente
    /// </summary>
    public void Modify(long id, string nombreUsuario, string correo, string telefono, string apodo, string contraseña)
    {
        var usuario = _repository.GetById(id);
        if (usuario == null)
            throw new Exception($"Usuario con Id {id} no encontrado");

        if (contraseña.Length < 6)
            throw new ArgumentException("La contraseña debe tener al menos 6 caracteres.");

        usuario.NombreUsuario = nombreUsuario.Trim();
        usuario.Correo = correo.Trim();
        usuario.Telefono = telefono;
        usuario.Apodo = apodo;
        usuario.Contraseña = contraseña;

        _repository.Update(usuario);
        _unitOfWork.SaveChanges();
    }

    /// <summary>
    /// Destroy - Elimina un usuario
    /// </summary>
    public void Destroy(long id)
    {
        var usuario = _repository.GetById(id);
        if (usuario == null)
            throw new Exception($"Usuario con Id {id} no encontrado");

        if (usuario.Correo.EndsWith("@admin.com"))
            throw new InvalidOperationException("No se puede eliminar un usuario administrador.");

        _repository.Delete(usuario);
        _unitOfWork.SaveChanges();
    }

    /// <summary>
    /// ReadOID - Obtiene un usuario por su ID
    /// </summary>
    public Usuario? ReadOID(long id) => _repository.GetById(id);

    /// <summary>
    /// ReadAll - Obtiene todos los usuarios
    /// </summary>
    public IEnumerable<Usuario> ReadAll() => _repository.GetAll();

    // ============== CUSTOM OPERATIONS ==============

    /// <summary>
    /// Login - Autentica un usuario por correo y contraseña
    /// </summary>
    public Usuario? Login(string correo, string contraseña)
    {
        if (string.IsNullOrWhiteSpace(correo))
            throw new ArgumentException("El correo es obligatorio");
        if (string.IsNullOrWhiteSpace(contraseña))
            throw new ArgumentException("La contraseña es obligatoria");

        var usuario = _repository.GetAll()
            .FirstOrDefault(u => u.Correo.ToLower() == correo.ToLower() && u.Contraseña == contraseña);

        if (usuario == null)
            throw new UnauthorizedAccessException("Correo o contraseña incorrectos");

        return usuario;
    }

    /// <summary>
    /// CUSTOM - Cambia la contraseña de un usuario con validación de contraseña actual
    /// </summary>
    public void CambiarContraseña(long id, string contraseñaActual, string contraseñaNueva)
    {
        var usuario = _repository.GetById(id);
        if (usuario == null)
            throw new Exception($"Usuario con Id {id} no encontrado");

        if (usuario.Contraseña != contraseñaActual)
            throw new UnauthorizedAccessException("La contraseña actual no es correcta");

        if (contraseñaNueva.Length < 6)
            throw new ArgumentException("La nueva contraseña debe tener al menos 6 caracteres");

        if (contraseñaNueva == contraseñaActual)
            throw new ArgumentException("La nueva contraseña debe ser diferente a la actual");

        usuario.Contraseña = contraseñaNueva;
        _repository.Update(usuario);
        _unitOfWork.SaveChanges();
    }

    // ============== FILTER OPERATIONS ==============

    /// <summary>
    /// ReadFilter - Filtra usuarios por nombre
    /// </summary>
    public IEnumerable<Usuario> ReadFilterByNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("Debe especificar un nombre para el filtro.");

        return _repository.GetAll()
            .Where(u => u.NombreUsuario.Contains(nombre, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    /// <summary>
    /// ReadFilter - Filtra usuarios por correo
    /// </summary>
    public IEnumerable<Usuario> ReadFilterByCorreo(string correo)
    {
        if (string.IsNullOrWhiteSpace(correo))
            throw new ArgumentException("Debe especificar un correo para el filtro.");

        return _repository.GetAll()
            .Where(u => u.Correo.Contains(correo, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    /// <summary>
    /// ReadFilter - Filtra usuarios por apodo
    /// </summary>
    public IEnumerable<Usuario> ReadFilterByApodo(string apodo)
    {
        if (string.IsNullOrWhiteSpace(apodo))
            throw new ArgumentException("Debe especificar un apodo para el filtro.");

        return _repository.GetAll()
            .Where(u => u.Apodo.Contains(apodo, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}
