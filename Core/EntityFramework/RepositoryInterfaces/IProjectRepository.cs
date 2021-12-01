namespace ProjectBank.Core.EF.Repository;

public interface IProjectRepository 
{
    Task<(Response, ProjectDTO)> CreateAsync(ProjectCreateDTO project);
    Task<ProjectDTO> ReadByIDAsyncAsync(int projectID);
    Task<ProjectDTO> ReadByKeyAsyncAsync(string ProjectTitle, string AuthorEmail);
    Task<ProjectDTO> ReadByStudentAsync(int studentID);
    Task<IReadOnlyCollection<ProjectDTO>> ReadAllAsync();
    Task<IReadOnlyCollection<ProjectDTO>> ReadAllAuthoredAsync(SupervisorDTO supervisor);
    Task<IReadOnlyCollection<ProjectDTO>> ReadAllByTag(TagDTO tag);
}