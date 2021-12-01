namespace ProjectBank.Core.EF.Repository;

public interface IUserRepository 
{
    Task<(Response, StudentDTO)> CreateAsync(StudentCreateDTO user);
    Task<(Response, SupervisorDTO)> CreateAsync(SupervisorCreateDTO user);
    Task<UserDTO> ReadByID(int userID);
    Task<UserDTO> ReadByEmail(string Email);
    Task<IReadOnlyCollection<UserDTO>> ReadAllAsync();
    Task<IReadOnlyCollection<StudentDTO>> ReadAllStudentsAsync();
    Task<IReadOnlyCollection<SupervisorDTO>> ReadAllSupervisorsAsync();
    Task<Response> UpdateStudentAsync(StudentDTO student);
    Task<Response> UpdateSupervisorAsync(SupervisorDTO student);
    Task<IReadOnlyCollection<ProjectDTO>> GetProjects(int userID);
    Task<IReadOnlyCollection<ProjectDTO>> GetAuthoredProjects(int userID);
}