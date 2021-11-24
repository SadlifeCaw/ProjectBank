namespace ProjectBank.Core.EF.Repository;

public interface IFacultyRepository 
{
    Task<FacultyDTO> CreateAsync(FacultyCreateDTO faculty);
    Task<FacultyDTO> ReadByIDAsync(int facultyID);
    Task<IReadOnlyCollection<FacultyDTO>> ReadAllAsync();
}