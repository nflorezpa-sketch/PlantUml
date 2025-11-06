namespace ApplicationCore.Domain.EN;

public class Vendedor : Usuario
{
    public virtual ISet<Videojuego> VideojuegosPublicados { get; set; } = new HashSet<Videojuego>();
}
