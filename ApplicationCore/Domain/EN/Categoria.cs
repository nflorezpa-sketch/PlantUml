namespace ApplicationCore.Domain.EN;

public class Categoria
{
    public virtual long Id { get; set; }
    public virtual string Nombre { get; set; } = string.Empty;
    public virtual string Descripcion { get; set; } = string.Empty;
    
    public virtual ISet<Videojuego> Videojuegos { get; set; } = new HashSet<Videojuego>();
}
