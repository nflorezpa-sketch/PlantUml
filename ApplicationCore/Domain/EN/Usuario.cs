namespace ApplicationCore.Domain.EN;

public class Usuario
{
    public virtual long Id { get; set; }
    public virtual string NombreUsuario { get; set; } = string.Empty;
    public virtual string Correo { get; set; } = string.Empty;
    public virtual string Telefono { get; set; } = string.Empty;
    public virtual string Apodo { get; set; } = string.Empty;
    public virtual string Contraseña { get; set; } = string.Empty;
    
    public virtual ISet<Reporte> Reportes { get; set; } = new HashSet<Reporte>();
    public virtual ISet<Soporte> Soportes { get; set; } = new HashSet<Soporte>();
    public virtual ISet<Pedido> Pedidos { get; set; } = new HashSet<Pedido>();
    public virtual ISet<Transaccion> Transacciones { get; set; } = new HashSet<Transaccion>();
    public virtual ISet<Videojuego> Videojuegos { get; set; } = new HashSet<Videojuego>();
    public virtual ISet<Insignia> Insignias { get; set; } = new HashSet<Insignia>();
}
