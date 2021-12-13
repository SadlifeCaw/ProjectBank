namespace ProjectBank.Core.EF.Repository;

public interface IUserRepository 
{
    Task<(Response, StudentDTO)> CreateAsync(StudentCreateDTO user);
    Task<(Response, SupervisorDTO)> CreateAsync(SupervisorCreateDTO user);
    Task<Option<UserDTO>> ReadByID(int userID);
    Task<Option<UserDTO>> ReadSupervisor(int userID);
    Task<Option<UserDTO>> ReadStudent(int userID);
    Task<UserDTO> ReadByEmail(string Email);
    Task<IReadOnlyCollection<UserDTO>> ReadAllAsync();
    Task<IReadOnlyCollection<StudentDTO>> ReadAllStudentsAsync();
    Task<IReadOnlyCollection<SupervisorDTO>> ReadAllSupervisorsAsync();
    Task<Response> UpdateUserProjects(int userID, ProjectKeyDTO project);
}