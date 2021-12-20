namespace ProjectBank.Infrastructure.ReferenceSystem;
public interface IProjectLSH
{
    Response Insert(IProject project);

    Task<IReadOnlyCollection<IProject>> GetSortedInCategory(IProject tagable);
    Task<IReadOnlyCollection<ProjectReferenceDTO>> GetSortedInCategory(IProject project, int size);
    Task<Response> InsertAll();
    Task<IReadOnlyCollection<IProject>> GetSorted(IProject tagable);
    Task<IReadOnlyCollection<ProjectReferenceDTO>> GetSorted(IProject project, int size);



}