namespace ProjectBank.Infrastructure;

public class UserRepository : IUserRepository
{
    private readonly ProjectBankContext _dbcontext;

    public UserRepository(ProjectBankContext context)
    {
        _dbcontext = context;
    }

    public async Task<(Response, StudentDTO)> CreateAsync(StudentCreateDTO user)
    {
        var conflict = await _dbcontext.Users.OfType<Student>()
                        .Where(u => u.Email == user.Email)
                        .Where(u => (u is Student))
                        .Select(u => new StudentDTO(u.Id, u.Email, u.FirstName, u.LastName, u.Program.Code, 
                                                    u.Program.Faculty.Institution.Title, u.Projects.Select(p => p.Id).ToList()))
                        .FirstOrDefaultAsync();

        if (conflict != null)
        {
            return (Response.Conflict, conflict);
        }

        var institution = await _dbcontext.Institutions
                              .Where(i => i.Title == user.InstitutionName)
                              .Select(i => i)
                              .FirstOrDefaultAsync();

        var program = await _dbcontext.Programs
                              .Where(p => p.Faculty.Institution == institution)
                              .Where(p => p.Code == user.ProgramCode)
                              .Select(p => p)
                              .FirstOrDefaultAsync();

        var entity = new Student
        {
            Email = user.Email,
            Institution = institution,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Projects = await GetProjectsAsync(user.ProjectIDs).ToListAsync(),
            Program = program
        };

        _dbcontext.Users.Add(entity);

        await _dbcontext.SaveChangesAsync();

        return (Response.Created, new StudentDTO(entity.Id, entity.Email, entity.FirstName, entity.LastName, entity.Program.Code, 
                                                 entity.Institution.Title, entity.Projects.Select(p => p.Id).ToList()));
    }

    public async Task<(Response, SupervisorDTO)> CreateAsync(SupervisorCreateDTO user)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyCollection<ProjectDTO>> GetAuthoredProjects(int userID)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyCollection<ProjectDTO>> GetProjects(int userID)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyCollection<UserDTO>> ReadAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyCollection<StudentDTO>> ReadAllStudentsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyCollection<SupervisorDTO>> ReadAllSupervisorsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<UserDTO> ReadByEmail(string Email)
    {
        throw new NotImplementedException();
    }

    public async Task<UserDTO> ReadByID(int userID)
    {
        throw new NotImplementedException();
    }

    public async Task<Response> UpdateStudentAsync(StudentDTO student)
    {
        throw new NotImplementedException();
    }

    public async Task<Response> UpdateSupervisorAsync(SupervisorDTO student)
    {
        throw new NotImplementedException();
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
}