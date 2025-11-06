namespace ApplicationCore.Domain.EN;

public class Moderador
{
    public virtual long Id { get; set; }
    public virtual string Correo { get; set; } = string.Empty;
    public virtual string Contraseña { get; set; } = string.Empty;
    
    public virtual ISet<Soporte> Soportes { get; set; } = new HashSet<Soporte>();
    public virtual ISet<Reporte> Reportes { get; set; } = new HashSet<Reporte>();
}
