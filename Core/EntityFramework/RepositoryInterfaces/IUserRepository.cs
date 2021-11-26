namespace ProjectBank.Core.EF.Repository;

public interface IUserRepository 
{
    Task<StudentDTO> CreateAsync(StudentCreateDTO student);
    Task<StudentDTO> ReadStudentByIDAsync(int studentID);
    Task<IReadOnlyCollection<StudentDTO>> ReadAllAsync();
}