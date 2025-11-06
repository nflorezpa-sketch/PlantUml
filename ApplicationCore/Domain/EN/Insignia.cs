using ApplicationCore.Domain.Enums;

namespace ApplicationCore.Domain.EN;

public class Insignia
{
    public virtual long Id { get; set; }
    public virtual TipoInsignia Perfil { get; set; }
    public virtual string RutaDelImg { get; set; } = string.Empty;
    
    public virtual Usuario? Usuario { get; set; }
    public virtual ISet<Desafio> Desafios { get; set; } = new HashSet<Desafio>();
}
