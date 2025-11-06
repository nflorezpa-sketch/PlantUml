using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;

namespace ApplicationCore.Domain.CEN;

public class ModeradorCEN
{
    private readonly IModeradorRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ModeradorCEN(IModeradorRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    // ============== CRUD OPERATIONS ==============

    /// <summary>
    /// New - Crea un nuevo moderador
    /// </summary>
    public long New(string correo, string contraseña)
    {
        if (string.IsNullOrWhiteSpace(correo) || !correo.Contains("@"))
            throw new ArgumentException("El correo no es válido");
        if (contraseña.Length < 8)
            throw new ArgumentException("La contraseña de moderador debe tener al menos 8 caracteres");

        var moderador = new Moderador
        {
            Correo = correo.Trim(),
            Contraseña = contraseña
        };
        _repository.Add(moderador);
        _unitOfWork.SaveChanges();
        return moderador.Id;
    }

    /// <summary>
    /// Modify - Modifica un moderador existente
    /// </summary>
    public void Modify(long id, string correo, string contraseña)
    {
        var moderador = _repository.GetById(id);
        if (moderador == null) throw new Exception($"Moderador con Id {id} no encontrado");

        if (contraseña.Length < 8)
            throw new ArgumentException("La contraseña de moderador debe tener al menos 8 caracteres");
        
        moderador.Correo = correo.Trim();
        moderador.Contraseña = contraseña;
        
        _repository.Update(moderador);
        _unitOfWork.SaveChanges();
    }

    /// <summary>
    /// Destroy - Elimina un moderador
    /// </summary>
    public void Destroy(long id)
    {
        var moderador = _repository.GetById(id);
        if (moderador == null) throw new Exception($"Moderador con Id {id} no encontrado");
        
        _repository.Delete(moderador);
        _unitOfWork.SaveChanges();
    }

    /// <summary>
    /// ReadOID - Obtiene un moderador por su ID
    /// </summary>
    public Moderador? ReadOID(long id) => _repository.GetById(id);

    /// <summary>
    /// ReadAll - Obtiene todos los moderadores
    /// </summary>
    public IEnumerable<Moderador> ReadAll() => _repository.GetAll();

    // ============== CUSTOM OPERATIONS ==============

    /// <summary>
    /// Login - Autentica un moderador por correo y contraseña
    /// </summary>
    public Moderador? Login(string correo, string contraseña)
    {
        if (string.IsNullOrWhiteSpace(correo))
            throw new ArgumentException("El correo es obligatorio");
        if (string.IsNullOrWhiteSpace(contraseña))
            throw new ArgumentException("La contraseña es obligatoria");

        var moderador = _repository.GetAll()
            .FirstOrDefault(m => m.Correo.ToLower() == correo.ToLower() && m.Contraseña == contraseña);

        if (moderador == null)
            throw new UnauthorizedAccessException("Correo o contraseña incorrectos");

        return moderador;
    }

    /// <summary>
    /// CUSTOM - Restablece la contraseña de un moderador con validación administrativa
    /// </summary>
    public void RestablecerContraseña(long moderadorId, string nuevaContraseña, string codigoAdmin)
    {
        // Validación: Solo administradores con código especial pueden restablecer contraseñas
        if (codigoAdmin != "ADMIN2025")
            throw new UnauthorizedAccessException("Código administrativo inválido");

        var moderador = _repository.GetById(moderadorId);
        if (moderador == null)
            throw new Exception($"Moderador con Id {moderadorId} no encontrado");

        if (nuevaContraseña.Length < 8)
            throw new ArgumentException("La contraseña de moderador debe tener al menos 8 caracteres");

        // Lógica especial: cambio forzado de contraseña con log en el correo
        moderador.Contraseña = nuevaContraseña;
        moderador.Correo = $"[RESET_{DateTime.Now:yyyyMMdd}]_{moderador.Correo.Replace("[RESET_", "").Split(']').Last()}";
        
        _repository.Update(moderador);
        _unitOfWork.SaveChanges();
    }

    // ============== FILTER OPERATIONS ==============

    /// <summary>
    /// ReadFilter - Filtra moderadores por correo
    /// </summary>
    public IEnumerable<Moderador> ReadFilterByCorreo(string correo)
    {
        if (string.IsNullOrWhiteSpace(correo))
            throw new ArgumentException("Debe especificar un correo para el filtro.");

        return _repository.GetAll()
            .Where(m => m.Correo.Contains(correo, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}
