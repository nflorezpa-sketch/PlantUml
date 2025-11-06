using ApplicationCore.Domain.Enums;

namespace ApplicationCore.Domain.EN;

public class Reporte
{
    public virtual long Id { get; set; }
    public virtual string UsuarioReportado { get; set; } = string.Empty;
    public virtual string Motivo { get; set; } = string.Empty;
    public virtual DateTime Fecha { get; set; }
    public virtual EstadoReporte Estado { get; set; }
    
    public virtual Usuario? Usuario { get; set; }
    public virtual ISet<Moderador> Moderadores { get; set; } = new HashSet<Moderador>();
}
