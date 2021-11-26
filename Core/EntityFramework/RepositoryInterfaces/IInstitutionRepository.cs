namespace ProjectBank.Core.EF.Repository;

public interface IInstitutionRepository 
{
    Task<(Response, InstitutionDTO)> CreateAsync(InstitutionCreateDTO institution);
    Task<InstitutionDTO> ReadFacultyByIDAsync(int insitutionID);
    Task<IReadOnlyCollection<InstitutionDTO>> ReadAllAsync();
}