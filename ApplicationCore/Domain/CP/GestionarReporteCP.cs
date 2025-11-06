using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using ApplicationCore.Domain.Enums;

namespace ApplicationCore.Domain.CP;

/// <summary>
/// Caso de uso: Gestionar reportes por parte de un moderador
/// Permite revisar y resolver reportes de usuarios
/// </summary>
public class GestionarReporteCP
{
    private readonly IReporteRepository _reporteRepository;
    private readonly IModeradorRepository _moderadorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GestionarReporteCP(
        IReporteRepository reporteRepository,
        IModeradorRepository moderadorRepository,
        IUnitOfWork unitOfWork)
    {
        _reporteRepository = reporteRepository;
        _moderadorRepository = moderadorRepository;
        _unitOfWork = unitOfWork;
    }

    public void Ejecutar(long moderadorId, long reporteId, EstadoReporte nuevoEstado)
    {
        _unitOfWork.BeginTransaction();
        
        try
        {
            // Validar moderador
            var moderador = _moderadorRepository.GetById(moderadorId);
            if (moderador == null)
                throw new Exception($"Moderador con Id {moderadorId} no encontrado");

            // Validar reporte
            var reporte = _reporteRepository.GetById(reporteId);
            if (reporte == null)
                throw new Exception($"Reporte con Id {reporteId} no encontrado");

            // Cambiar estado del reporte
            reporte.Estado = nuevoEstado;
            _reporteRepository.Update(reporte);

            // Asociar moderador al reporte si no está ya asociado
            if (!moderador.Reportes.Contains(reporte))
            {
                moderador.Reportes.Add(reporte);
                _moderadorRepository.Update(moderador);
            }

            _unitOfWork.Commit();
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
}
