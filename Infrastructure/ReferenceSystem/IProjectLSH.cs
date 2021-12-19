using ProjectBank.Infrastructure.Entities;

namespace ProjectBank.Infrastructure.ReferenceSystem;
public interface IProjectLSH
{
    Task<Response> Insert(IProject project);

    Task<IReadOnlyCollection<IProject>> GetSortedInCategory(IProject tagable);
    Task<Response> InsertAll();
    Task<IReadOnlyCollection<IProject>> GetSorted(IProject tagable);
    Task<IReadOnlyCollection<ProjectReferenceDTO>> GetSorted(int id, int size);



}