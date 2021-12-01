namespace ProjectBank.Core.EF.Repository;

public interface IProjectRepository 
{
    Task<(Response, ProjectDTO)> CreateAsync(ProjectCreateDTO project);
    Task<ProjectDTO> ReadByIDAsyncAsync(int projectID);
    Task<ProjectDTO> ReadByStudentAsync(int studentID);
    Task<IReadOnlyCollection<ProjectDTO>> ReadAllAsync();
    Task<IReadOnlyCollection<ProjectDTO>> ReadAllAuthoredAsync(SupervisorDTO supervisor);
}