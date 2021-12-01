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
        throw new NotImplementedException();
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
}