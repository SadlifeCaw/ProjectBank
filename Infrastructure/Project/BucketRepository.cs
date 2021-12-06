namespace ProjectBank.Infrastructure.Entities;

 public class BucketRepository : IBucketRepository
{
    private readonly ProjectBankContext _dbcontext;

    public BucketRepository(ProjectBankContext context)
    {
        _dbcontext = context;
    }

    public async Task<(Response, BucketDTO)> CreateAsync(BucketCreateDTO bucket)
    {
        var conflict =
            await (_dbcontext.Buckets
                            .Where(b => b.Key == bucket.Key)
                            .Where(b => b.Projects == bucket.ProjectIds)
                            .Select(b => new BucketDTO(b.Projects.Select(b => b.Id).ToHashSet(),b.Key)))
                            .FirstOrDefaultAsync();

        if (conflict != null)
        {
            return (Response.Conflict, conflict);
            
        }

        var entity = new ProjectBucket(await GetProjectsAsync(bucket.ProjectIds).ToHashSetAsync(),
                                       bucket.Key);

        _dbcontext.Buckets.Add(entity);

        await _dbcontext.SaveChangesAsync();

        return (Response.Created, new BucketDTO(entity.Projects.Select(p => p.Id).ToHashSet(), entity.Key));
    }
    public async Task<BucketDTO> ReadBucketByKeyAsync(string key)
    {
        var buckets = from b in _dbcontext.Buckets
                      where b.Key == key
                      select new BucketDTO(b.Projects.Select(p => p.Id).ToHashSet(), b.Key);

        return await buckets.FirstOrDefaultAsync();
    }

    public async Task<BucketDTO> ReadBucketByIDAsync(int bucketId)
    {
        var buckets = from b in _dbcontext.Buckets
                      where b.Id == bucketId
                      select new BucketDTO(b.Projects.Select(p => p.Id).ToHashSet(), b.Key);

        return await buckets.FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<BucketDTO>> ReadAllAsync() =>
        (await _dbcontext.Buckets
                        .Select(b => new BucketDTO(b.Projects.Select(p => p.Id).ToHashSet(),b.Key))
                        .ToListAsync())
                        .AsReadOnly();

    private async IAsyncEnumerable<Project> GetProjectsAsync(ICollection<int> inProjects)
    {
        var existing = await _dbcontext.Projects
                        .Where(p => inProjects
                                    .Any(inP => inP == p.Id))
                        .Select(p => p)
                        .ToListAsync();
                           
        foreach (var project in existing)
        {
            yield return project;
        }
    }

    public async Task<Response> AddProjectAsync(int projectID)
    {
        throw new NotImplementedException();
    }

    public async Task<Response> RemoveProjectAsync(int projectID)
    {
        throw new NotImplementedException();
    }

    public async Task<Response> UpdateAllProjectAsync(ICollection<int> projectIDs)
    {
        throw new NotImplementedException();
    }
}