using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using ApplicationCore.Domain.Enums;

namespace ApplicationCore.Domain.CP;

/// <summary>
/// Caso de uso: Reportar a otro usuario
/// Crea un reporte con motivo y lo asocia al usuario que reporta
/// </summary>
public class ReportarUsuarioCP
{
    private readonly IReporteRepository _reporteRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReportarUsuarioCP(
        IReporteRepository reporteRepository,
        IUsuarioRepository usuarioRepository,
        IUnitOfWork unitOfWork)
    {
        _reporteRepository = reporteRepository;
        _usuarioRepository = usuarioRepository;
        _unitOfWork = unitOfWork;
    }

    public long Ejecutar(long usuarioReportanteId, string nombreUsuarioReportado, string motivo)
    {
        _unitOfWork.BeginTransaction();
        
        try
        {
            // Validar usuario que reporta
            var usuarioReportante = _usuarioRepository.GetById(usuarioReportanteId);
            if (usuarioReportante == null)
                throw new Exception($"Usuario con Id {usuarioReportanteId} no encontrado");

            // Validar que el usuario reportado existe
            var usuarioReportado = _usuarioRepository.GetAll()
                .FirstOrDefault(u => u.NombreUsuario == nombreUsuarioReportado);
            
            if (usuarioReportado == null)
                throw new Exception($"Usuario reportado '{nombreUsuarioReportado}' no encontrado");

            // No permitir auto-reportes
            if (usuarioReportante.Id == usuarioReportado.Id)
                throw new InvalidOperationException("No puede reportarse a sí mismo");

            // Validar motivo
            if (string.IsNullOrWhiteSpace(motivo))
                throw new ArgumentException("Debe proporcionar un motivo para el reporte");

            // Crear el reporte
            var reporte = new Reporte
            {
                UsuarioReportado = nombreUsuarioReportado,
                Motivo = motivo.Trim(),
                Fecha = DateTime.Now,
                Estado = EstadoReporte.SinSolucionar,
                Usuario = usuarioReportante
            };

            _reporteRepository.Add(reporte);
            _unitOfWork.Commit();
            
            return reporte.Id;
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
}
