using ApplicationCore.Domain.Enums;

namespace ApplicationCore.Domain.EN;

public class Soporte
{
    public virtual long Id { get; set; }
    public virtual string Descripcion { get; set; } = string.Empty;
    public virtual EstadoSoporte Estado { get; set; }
    
    public virtual Usuario? Usuario { get; set; }
    public virtual ISet<Moderador> Moderadores { get; set; } = new HashSet<Moderador>();
}
