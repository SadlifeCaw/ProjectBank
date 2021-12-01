namespace ProjectBank.Infrastructure;

public class UserRepository : IUserRepository
{
    private readonly ProjectBankContext _dbcontext;

    public UserRepository(ProjectBankContext context)
    {
        _dbcontext = context;
    }

    public Task<(Response, StudentDTO)> CreateAsync(StudentCreateDTO user)
    {
        throw new NotImplementedException();
    }

    public Task<(Response, SupervisorDTO)> CreateAsync(SupervisorCreateDTO user)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<ProjectDTO>> GetAuthoredProjects(int userID)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<ProjectDTO>> GetProjects(int userID)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<UserDTO>> ReadAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<StudentDTO>> ReadAllStudentsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<SupervisorDTO>> ReadAllSupervisorsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO> ReadByEmail(string Email)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO> ReadByID(int userID)
    {
        throw new NotImplementedException();
    }

    public Task<Response> UpdateStudentAsync(StudentDTO student)
    {
        throw new NotImplementedException();
    }

    public Task<Response> UpdateSupervisorAsync(SupervisorDTO student)
    {
        throw new NotImplementedException();
    }
}