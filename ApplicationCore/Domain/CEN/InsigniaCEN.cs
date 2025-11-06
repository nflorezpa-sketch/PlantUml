using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using ApplicationCore.Domain.Enums;

namespace ApplicationCore.Domain.CEN;

public class InsigniaCEN
{
    private readonly IInsigniaRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public InsigniaCEN(IInsigniaRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    // ============== CRUD OPERATIONS ==============

    /// <summary>
    /// New - Crea una nueva insignia
    /// </summary>
    public long New(TipoInsignia perfil, string rutaDelImg)
    {
        var insignia = new Insignia
        {
            Perfil = perfil,
            RutaDelImg = rutaDelImg
        };
        _repository.Add(insignia);
        _unitOfWork.SaveChanges();
        return insignia.Id;
    }

    /// <summary>
    /// Modify - Modifica una insignia existente
    /// </summary>
    public void Modify(long id, TipoInsignia perfil, string rutaDelImg)
    {
        var insignia = _repository.GetById(id);
        if (insignia == null) throw new Exception($"Insignia con Id {id} no encontrado");
        
        insignia.Perfil = perfil;
        insignia.RutaDelImg = rutaDelImg;
        
        _repository.Update(insignia);
        _unitOfWork.SaveChanges();
    }

    /// <summary>
    /// Destroy - Elimina una insignia
    /// </summary>
    public void Destroy(long id)
    {
        var insignia = _repository.GetById(id);
        if (insignia == null) throw new Exception($"Insignia con Id {id} no encontrado");
        
        _repository.Delete(insignia);
        _unitOfWork.SaveChanges();
    }

    /// <summary>
    /// ReadOID - Obtiene una insignia por su ID
    /// </summary>
    public Insignia? ReadOID(long id)
    {
        return _repository.GetById(id);
    }

    /// <summary>
    /// ReadAll - Obtiene todas las insignias
    /// </summary>
    public IEnumerable<Insignia> ReadAll()
    {
        return _repository.GetAll();
    }

    // ============== READ FILTERS ==============

    /// <summary>
    /// ReadFilterByTipo - Obtiene insignias filtradas por tipo
    /// </summary>
    public IEnumerable<Insignia> ReadFilterByTipo(TipoInsignia tipo)
    {
        return _repository.GetAll().Where(i => i.Perfil == tipo);
    }
}
