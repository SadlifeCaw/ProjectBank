namespace ProjectBank.Core.EF.Repository;

public interface IUserRepository 
{
    Task<(Response, StudentDTO)> CreateAsync(StudentCreateDTO user);
    Task<(Response, SupervisorDTO)> CreateAsync(SupervisorCreateDTO user);
    Task<UserDTO> ReadByID(int userID);
    Task<IReadOnlyCollection<UserDTO>> ReadAllAsync();
    Task<IReadOnlyCollection<StudentDTO>> ReadAllStudentsAsync();
    Task<IReadOnlyCollection<SupervisorDTO>> ReadAllSupervisorsAsync();
}