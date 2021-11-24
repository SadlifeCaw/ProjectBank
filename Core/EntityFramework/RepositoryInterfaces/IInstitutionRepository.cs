namespace ProjectBank.Core.EF.Repository;

public interface IInstitutionRepository 
{
    Task<InstitutionDTO> CreateAsync(InstitutionCreateDTO institution);
    Task<InstitutionDTO> ReadFacultyByIDAsync(int insitutionID);
    Task<IReadOnlyCollection<InstitutionDTO>> ReadAllAsync();
}