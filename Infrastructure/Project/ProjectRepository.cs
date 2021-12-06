namespace ProjectBank.Infrastructure;

public class ProjectRepository : IProjectRepository
{
    private readonly ProjectBankContext _dbcontext;

    public ProjectRepository(ProjectBankContext context)
    {
        _dbcontext = context;
    }

    public async Task<(Response, ProjectDTO)> CreateAsync(ProjectCreateDTO project)
    {
        var conflict = await _dbcontext.Projects
                        .Where(p => p.Author.Id == project.AuthorID)
                        .Where(p => p.Title == project.Title)
                        .Select(p => new ProjectDTO(p.Id, p.Author.Id, p.Title, p.Description, p.Status,
                                                    p.Tags.Select(t => t.Id).ToList(), p.Users.Select(u => u.Id).ToList()))
                        .FirstOrDefaultAsync();

        if (conflict != null)
        {
            return (Response.Conflict, conflict);
        }

        var author = await GetSupervisorAsync(project.AuthorID);

        var entity = new Project
        {
            Author = author,
            Title = project.Title,
            Description = project.Description,
            Status = project.Status,
            Tags = await GetTagsAsync(project.TagIDs).ToListAsync(),
            Users = await GetUsersAsync(project.UserIDs).ToListAsync()
        };

        _dbcontext.Projects.Add(entity);

        await _dbcontext.SaveChangesAsync();

        return (Response.Created, new ProjectDTO(entity.Id, entity.Author.Id, entity.Title, entity.Description, entity.Status,
                                                 entity.Tags.Select(t => t.Id).ToList(), entity.Users.Select(u => u.Id).ToList()));
    }
    public async Task<IReadOnlyCollection<ProjectDTO>> ReadAllAsync()
    {
        return (await _dbcontext.Projects
                        .Select(p => new ProjectDTO(p.Id, p.Author.Id, p.Title, p.Description, p.Status,
                                                    p.Tags.Select(t => t.Id).ToList(), p.Users.Select(u => u.Id).ToList()))
                        .ToListAsync())
                        .AsReadOnly();
    }

    public async Task<IReadOnlyCollection<ProjectDTO>> ReadAllAuthoredAsync(int authorID)
    {
        return (await _dbcontext.Projects
                        .Where(p => p.Author.Id == authorID)
                        .Select(p => new ProjectDTO(p.Id, p.Author.Id, p.Title, p.Description, p.Status,
                                          p.Tags.Select(t => t.Id).ToList(), p.Users.Select(u => u.Id).ToList()))
                        .ToListAsync())
                        .AsReadOnly();
    }

    public async Task<IReadOnlyCollection<ProjectDTO>> ReadAllByTagAsync(int tagID)
    {
        return (await _dbcontext.Projects
                        .Where(p => p.Tags
                                    .Any(t => t.Id == tagID))
                        .Select(p => new ProjectDTO(p.Id, p.Author.Id, p.Title, p.Description, p.Status,
                                                    p.Tags.Select(t => t.Id).ToList(), p.Users.Select(u => u.Id).ToList()))
                        .ToListAsync())
                        .AsReadOnly();
    }

    public async Task<ProjectDTO> ReadByIDAsync(int projectID)
    {
        var users = from p in _dbcontext.Projects
                    where p.Id == projectID
                    select new ProjectDTO(p.Id, p.Author.Id, p.Title, p.Description, p.Status,
                                          p.Tags.Select(t => t.Id).ToList(), p.Users.Select(u => u.Id).ToList());

        return await users.FirstOrDefaultAsync(); 
    }

    public async Task<ProjectDTO> ReadByKeyAsync(string ProjectTitle, int authorID)
    {
        var users = from p in _dbcontext.Projects
                    where p.Author.Id == authorID
                    where p.Title == ProjectTitle
                    select new ProjectDTO(p.Id, p.Author.Id, p.Title, p.Description, p.Status,
                                          p.Tags.Select(t => t.Id).ToList(), p.Users.Select(u => u.Id).ToList());

        return await users.FirstOrDefaultAsync(); 
    }

    public async Task<Response> AddUserAsync(ProjectKeyDTO projectKey, int userID)
    {
        var user = await GetUserAsync(userID);

        var project = await _dbcontext.Projects
                            .Where(p => p.Author.Id == projectKey.AuthorID)
                            .Where(p => p.Title == projectKey.Title)
                            .Select(p => p)
                            .FirstOrDefaultAsync();

        if(user == null || project == null)
        {
            return Response.NotFound;
        }

        user.Projects.Add(project);

        await _dbcontext.SaveChangesAsync();

        return Response.Updated;
    }

    public async Task<Response> RemoveUserAsync(ProjectKeyDTO projectKey, int userID)
    {
        var user = await GetUserAsync(userID);

        var project = await _dbcontext.Projects
                            .Where(p => p.Author.Id == projectKey.AuthorID)
                            .Where(p => p.Title == projectKey.Title)
                            .Select(p => p)
                            .FirstOrDefaultAsync();

        if(user == null || project == null)
        {
            return Response.NotFound;
        }

        var removed = user.Projects.Remove(project);

        if(removed) 
        {
            await _dbcontext.SaveChangesAsync();
            return Response.Updated;
        } 

        return Response.NotFound;
    }

    public async Task<Response> UpdateAsync(ProjectUpdateDTO project)
    {
        throw new NotImplementedException();
    }

    private async Task<Supervisor> GetSupervisorAsync(int authorID)
    {
        var users = from u in _dbcontext.Users.OfType<Supervisor>()
                    where u.Id == authorID
                    select u;
                           
        return await users.FirstOrDefaultAsync();
    }

    private async IAsyncEnumerable<Tag> GetTagsAsync(ICollection<int> inTags)
    {
        var existing = await _dbcontext.Tags
                        .Where(t => inTags
                                    .Any(inT => inT == t.Id))
                        .Select(t => t)
                        .ToListAsync();
                           
        foreach (var tag in existing)
        {
            yield return tag;
        }
    }

    private async IAsyncEnumerable<User> GetUsersAsync(ICollection<int> inUsers)
    {
        var existing = await _dbcontext.Users.OfType<Student>()
                        .Where(u => inUsers
                                    .Any(inS => inS == u.Id))
                        .Select(u => u)
                        .ToListAsync();
                           
        foreach (var user in existing)
        {
            yield return user;
        }
    }

    private async Task<User> GetUserAsync(int userID)
    {
        return await _dbcontext.Users
                        .Where(u => u.Id == userID)
                        .Select(u => u)
                        .FirstOrDefaultAsync();
    }

    private async IAsyncEnumerable<Supervisor> GetCollaboratorsAsync(ICollection<int> inUsers)
    {
        var existing = await _dbcontext.Users.OfType<Supervisor>()
                        .Where(u => inUsers
                                    .Any(inS => inS == u.Id))
                        .Select(u => u)
                        .ToListAsync();
                           
        foreach (var user in existing)
        {
            yield return user;
        }
    }
}