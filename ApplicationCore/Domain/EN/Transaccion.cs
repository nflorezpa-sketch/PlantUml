using ApplicationCore.Domain.Enums;

namespace ApplicationCore.Domain.EN;

public class Transaccion
{
    public virtual long Id { get; set; }
    public virtual DateTime Fecha { get; set; }
    public virtual decimal Total { get; set; }
    public virtual string MetodoPago { get; set; } = string.Empty;
    public virtual TipoOperacion TipoOperacion { get; set; }
    
    public virtual Usuario Usuario { get; set; } = null!;
    public virtual Pedido? Pedido { get; set; }
}
