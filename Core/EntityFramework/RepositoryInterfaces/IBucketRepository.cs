namespace ProjectBank.Core.EF.Repository;

public interface IBucketRepository
{
    Task<(Response, BucketDTO)> CreateAsync(BucketCreateDTO course);
    Task<BucketDTO> ReadBucketByIDAsync(int bucketId);
    Task<BucketDTO> ReadBucketByKeyAsync(string key);
    Task<Response> AddProjectAsync(int projectID); 
    Task<Response> RemoveProjectAsync(int projectID);
    Task<Response> UpdateAllProjectAsync(ICollection<int> projectIDs);  
    Task<IReadOnlyCollection<BucketDTO>> ReadAllAsync();
}