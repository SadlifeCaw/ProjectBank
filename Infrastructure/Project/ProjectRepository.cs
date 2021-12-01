namespace ProjectBank.Infrastructure;

public class ProjectRepository : IProjectRepository
{
    public Task<(Response, ProjectDTO)> CreateAsync(ProjectCreateDTO project)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<ProjectDTO>> ReadAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<ProjectDTO>> ReadAllAuthoredAsync(SupervisorDTO supervisor)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<ProjectDTO>> ReadAllByTag(TagDTO tag)
    {
        throw new NotImplementedException();
    }

    public Task<ProjectDTO> ReadByIDAsyncAsync(int projectID)
    {
        throw new NotImplementedException();
    }

    public Task<ProjectDTO> ReadByKeyAsyncAsync(string ProjectTitle, string AuthorEmail)
    {
        throw new NotImplementedException();
    }

    public Task<ProjectDTO> ReadByStudentAsync(int studentID)
    {
        throw new NotImplementedException();
    }
}