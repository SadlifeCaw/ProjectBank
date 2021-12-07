namespace ProjectBank.Core.EF.Repository;

public interface IProjectRepository 
{
    Task<(Response, ProjectDTO)> CreateAsync(ProjectCreateDTO project);
    Task<ProjectDTO> ReadByIDAsync(int projectID);
    Task<ProjectDTO> ReadByKeyAsync(string ProjectTitle, int authorID);
    Task<IReadOnlyCollection<ProjectDTO>> ReadAllAsync();
    Task<IReadOnlyCollection<ProjectDTO>> ReadAllAuthoredAsync(int authorID);
    Task<IReadOnlyCollection<ProjectDTO>> ReadAllByTagAsync(int tagID);
    Task<IReadOnlyCollection<ProjectDTO>> ReadCollectionAsync(ICollection<int> projectIDs);
    Task<Response> UpdateAsync(ProjectUpdateDTO project);
    Task<Response> AddUserAsync(ProjectKeyDTO projectKey, int userID);
    Task<Response> RemoveUserAsync(ProjectKeyDTO projectKey, int userID);
}