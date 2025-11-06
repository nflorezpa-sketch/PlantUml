using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using ApplicationCore.Domain.Enums;

namespace ApplicationCore.Domain.CEN;

public class ReporteCEN
{
    private readonly IReporteRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ReporteCEN(IReporteRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    // ============== CRUD OPERATIONS ==============

    /// <summary>
    /// New - Crea un nuevo reporte
    /// </summary>
    public long New(string usuarioReportado, string motivo, DateTime fecha, EstadoReporte estado)
    {
        if (string.IsNullOrWhiteSpace(usuarioReportado))
            throw new ArgumentException("El usuario reportado no puede estar vacío.");
        if (string.IsNullOrWhiteSpace(motivo))
            throw new ArgumentException("El motivo no puede estar vacío.");
        if (fecha > DateTime.Now)
            throw new ArgumentException("La fecha del reporte no puede ser futura.");

        var reporte = new Reporte
        {
            UsuarioReportado = usuarioReportado,
            Motivo = motivo,
            Fecha = fecha,
            Estado = estado
        };

        _repository.Add(reporte);
        _unitOfWork.SaveChanges();
        return reporte.Id;
    }

    /// <summary>
    /// Modify - Modifica un reporte existente
    /// </summary>
    public void Modify(long id, string usuarioReportado, string motivo, DateTime fecha, EstadoReporte estado)
    {
        var reporte = _repository.GetById(id);
        if (reporte == null)
            throw new Exception($"Reporte con Id {id} no encontrado");

        if (string.IsNullOrWhiteSpace(motivo))
            throw new ArgumentException("El motivo del reporte no puede estar vacío.");
        if (fecha > DateTime.Now)
            throw new ArgumentException("La fecha no puede ser futura.");

        reporte.UsuarioReportado = usuarioReportado;
        reporte.Motivo = motivo;
        reporte.Fecha = fecha;
        reporte.Estado = estado;

        _repository.Update(reporte);
        _unitOfWork.SaveChanges();
    }

    /// <summary>
    /// Destroy - Elimina un reporte
    /// </summary>
    public void Destroy(long id)
    {
        var reporte = _repository.GetById(id);
        if (reporte == null)
            throw new Exception($"Reporte con Id {id} no encontrado");

        if (reporte.Estado == EstadoReporte.Solucionado)
            throw new InvalidOperationException("No se puede eliminar un reporte solucionado.");

        _repository.Delete(reporte);
        _unitOfWork.SaveChanges();
    }

    /// <summary>
    /// ReadOID - Obtiene un reporte por su ID
    /// </summary>
    public Reporte? ReadOID(long id)
    {
        return _repository.GetById(id);
    }

    /// <summary>
    /// ReadAll - Obtiene todos los reportes
    /// </summary>
    public IEnumerable<Reporte> ReadAll()
    {
        return _repository.GetAll();
    }

    // ============== READ FILTERS ==============

    /// <summary>
    /// ReadFilterByEstado - Obtiene reportes filtrados por estado
    /// </summary>
    public IEnumerable<Reporte> ReadFilterByEstado(EstadoReporte estado)
    {
        return _repository.GetAll().Where(r => r.Estado == estado);
    }

    /// <summary>
    /// ReadFilterByFecha - Obtiene reportes filtrados por rango de fechas
    /// </summary>
    public IEnumerable<Reporte> ReadFilterByFecha(DateTime desde, DateTime hasta)
    {
        return _repository.GetAll().Where(r => r.Fecha >= desde && r.Fecha <= hasta);
    }
}
