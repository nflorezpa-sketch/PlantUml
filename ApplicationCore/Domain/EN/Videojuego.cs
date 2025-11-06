namespace ApplicationCore.Domain.EN;

public class Videojuego
{
    public virtual long Id { get; set; }
    public virtual float Precio { get; set; }
    
    public virtual Categoria? Categoria { get; set; }
    public virtual ISet<Usuario> Usuarios { get; set; } = new HashSet<Usuario>();
    public virtual ISet<Vendedor> Vendedores { get; set; } = new HashSet<Vendedor>();
    public virtual ISet<Pedido> Pedidos { get; set; } = new HashSet<Pedido>();
    public virtual ISet<Desafio> Desafios { get; set; } = new HashSet<Desafio>();
}
