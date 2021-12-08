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

     public async Task<Response> AddProjectAsync(int bucketID, int projectID)
    {
        var bucket = await (_dbcontext.Buckets
                        .Where(b => b.Id == bucketID)
                        .Select(b => b))
                        .FirstOrDefaultAsync();

        var project = await (_dbcontext.Projects
                        .Where(p => p.Id == projectID)
                        .Select(p => p))
                        .FirstOrDefaultAsync();

        if(bucket == null || project == null)
        {
            return Response.NotFound;
        }

        bucket.Projects.Add(project);

        return Response.Updated;
    }

    public async Task<Response> RemoveProjectAsync(int bucketID, int projectID)
    {
        var bucket = await (_dbcontext.Buckets
                        .Where(b => b.Id == bucketID)
                        .Select(b => b))
                        .FirstOrDefaultAsync();

        if(bucket == null)
        {
            return Response.NotFound;
        }

        //find project in bucket - if it actually holds it
        var project = (bucket.Projects
                        .Where(p => p.Id == projectID)
                        .Select(p => p))
                        .FirstOrDefault();
        
        if(project == null)
        {
            return Response.NotFound;
        }

        var removed = bucket.Projects.Remove(project);

        if(removed) 
        {
            await _dbcontext.SaveChangesAsync();
            return Response.Updated;
        } 

        return Response.BadRequest;
    }

    //changes a bucket's projects to another set of projects.
    //If none of the project ID exists in the database, then refuse. 
    //To clear the bucket use ClearBucketAsync
    public async Task<Response> UpdateAllProjectAsync(int bucketID, ICollection<int> projectIDs)
    {
        var bucket = await (_dbcontext.Buckets
                        .Where(b => b.Id == bucketID)
                        .Select(b => b))
                        .FirstOrDefaultAsync();

        if(bucket == null)
        {
            return Response.NotFound;
        }

        var projects = await GetProjectsAsync(projectIDs).ToHashSetAsync();

        if(projects.Count() == 0)
        {
           return Response.BadRequest; 
        }

        bucket.Projects.Clear();
        
        foreach (var project in projects)
        {
            bucket.Projects.Add(project);
        }

        await _dbcontext.SaveChangesAsync();
        return Response.Updated;
    }

    public async Task<Response> ClearBucketAsync(int bucketID)
    {
        var bucket = await (_dbcontext.Buckets
                        .Where(b => b.Id == bucketID)
                        .Select(b => b))
                        .FirstOrDefaultAsync();

        if(bucket == null)
        {
            return Response.NotFound;
        }

        bucket.Projects.Clear();

        await _dbcontext.SaveChangesAsync();
        return Response.Updated;
    }

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

    private async Task<Project> GetProjectAsync(int projectID)
    {
        return await _dbcontext.Projects
                    .Where(p => p.Id == projectID)
                    .Select(p => p)
                    .FirstOrDefaultAsync();
    }
}