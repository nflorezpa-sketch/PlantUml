namespace ApplicationCore.Domain.EN;

public class Pedido
{
    public virtual long Id { get; set; }
    public virtual decimal Total { get; set; }
    
    public virtual Usuario Usuario { get; set; } = null!;
    public virtual ISet<Videojuego> Videojuegos { get; set; } = new HashSet<Videojuego>();
    public virtual Transaccion? Transaccion { get; set; }
}
