namespace ProjectBank.Core.EF.Repository;

public interface IProgramRepository 
{
    Task<(Response, ProgramDTO)> CreateAsync(ProgramCreateDTO program);
    Task<ProgramDTO> ReadProgramByIDAsync(int programID);
    Task<IReadOnlyCollection<ProgramDTO>> ReadAllAsync();
}

