using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;

namespace ApplicationCore.Domain.CEN;

public class VendedorCEN
{
    private readonly IVendedorRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public VendedorCEN(IVendedorRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    // ============== CRUD OPERATIONS ==============

    /// <summary>
    /// New - Crea un nuevo vendedor
    /// </summary>
    public long New(string nombreUsuario, string correo, string telefono, string apodo, string contraseña)
    {
        if (string.IsNullOrWhiteSpace(nombreUsuario))
            throw new ArgumentException("El nombre de usuario no puede estar vacío.");
        if (!correo.Contains("@"))
            throw new ArgumentException("El correo electrónico no es válido.");
        if (contraseña.Length < 6)
            throw new ArgumentException("La contraseña debe tener al menos 6 caracteres.");

        var vendedor = new Vendedor
        {
            NombreUsuario = nombreUsuario.Trim(),
            Correo = correo.Trim(),
            Telefono = telefono,
            Apodo = apodo,
            Contraseña = contraseña
        };

        _repository.Add(vendedor);
        _unitOfWork.SaveChanges();
        return vendedor.Id;
    }

    /// <summary>
    /// Modify - Modifica un vendedor existente
    /// </summary>
    public void Modify(long id, string nombreUsuario, string correo, string telefono, string apodo, string contraseña)
    {
        var vendedor = _repository.GetById(id);
        if (vendedor == null)
            throw new Exception($"Vendedor con Id {id} no encontrado");

        if (!correo.Contains("@"))
            throw new ArgumentException("El correo electrónico no es válido.");
        if (contraseña.Length < 6)
            throw new ArgumentException("Contraseña demasiado corta.");

        vendedor.NombreUsuario = nombreUsuario.Trim();
        vendedor.Correo = correo.Trim();
        vendedor.Telefono = telefono;
        vendedor.Apodo = apodo;
        vendedor.Contraseña = contraseña;

        _repository.Update(vendedor);
        _unitOfWork.SaveChanges();
    }

    /// <summary>
    /// Destroy - Elimina un vendedor
    /// </summary>
    public void Destroy(long id)
    {
        var vendedor = _repository.GetById(id);
        if (vendedor == null)
            throw new Exception($"Vendedor con Id {id} no encontrado");

        if (vendedor.Correo.Contains("soporte"))
            throw new InvalidOperationException("No se puede eliminar una cuenta de soporte.");

        _repository.Delete(vendedor);
        _unitOfWork.SaveChanges();
    }

    /// <summary>
    /// ReadOID - Obtiene un vendedor por su ID
    /// </summary>
    public Vendedor? ReadOID(long id) => _repository.GetById(id);

    /// <summary>
    /// ReadAll - Obtiene todos los vendedores
    /// </summary>
    public IEnumerable<Vendedor> ReadAll() => _repository.GetAll();

    // ============== CUSTOM OPERATIONS ==============

    /// <summary>
    /// Login - Autentica un vendedor por correo y contraseña
    /// </summary>
    public Vendedor? Login(string correo, string contraseña)
    {
        if (string.IsNullOrWhiteSpace(correo))
            throw new ArgumentException("El correo es obligatorio");
        if (string.IsNullOrWhiteSpace(contraseña))
            throw new ArgumentException("La contraseña es obligatoria");

        var vendedor = _repository.GetAll()
            .FirstOrDefault(v => v.Correo.ToLower() == correo.ToLower() && v.Contraseña == contraseña);

        if (vendedor == null)
            throw new UnauthorizedAccessException("Correo o contraseña incorrectos");

        return vendedor;
    }

    /// <summary>
    /// CUSTOM - Suspende temporalmente la cuenta de un vendedor
    /// </summary>
    public void SuspenderCuenta(long vendedorId, string motivo)
    {
        var vendedor = _repository.GetById(vendedorId);
        if (vendedor == null)
            throw new Exception($"Vendedor con Id {vendedorId} no encontrado");

        if (string.IsNullOrWhiteSpace(motivo))
            throw new ArgumentException("Debe especificar un motivo para la suspensión");

        // Lógica especial: marcamos al vendedor como suspendido cambiando su correo
        vendedor.Correo = $"SUSPENDIDO_{motivo}_{vendedor.Correo}";
        vendedor.Contraseña = Guid.NewGuid().ToString(); // Genera contraseña aleatoria para bloquear acceso
        
        _repository.Update(vendedor);
        _unitOfWork.SaveChanges();
    }

    // ============== FILTER OPERATIONS ==============

    /// <summary>
    /// ReadFilter - Filtra vendedores por correo
    /// </summary>
    public IEnumerable<Vendedor> ReadFilterByCorreo(string correo)
    {
        if (string.IsNullOrWhiteSpace(correo))
            throw new ArgumentException("Debe especificar un correo para el filtro.");

        return _repository.GetAll()
            .Where(v => v.Correo.Contains(correo, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}
