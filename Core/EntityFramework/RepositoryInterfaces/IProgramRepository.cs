namespace ProjectBank.Core.EF.Repository;

public interface IProgramRepository 
{
    Task<ProgramDTO> CreateAsync(ProgramCreateDTO program);
    Task<ProgramDTO> ReadFacultyByIDAsync(int programID);
    Task<IReadOnlyCollection<ProgramDTO>> ReadAllAsync();
}

