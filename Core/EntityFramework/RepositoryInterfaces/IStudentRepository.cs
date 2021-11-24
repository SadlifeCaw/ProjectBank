namespace ProjectBank.Core.EF.Repository;

public interface IStudentRepository 
{
    Task<StudentDTO> CreateAsync(StudentCreateDTO student);
    Task<StudentDTO> ReadStudentByIDAsync(int studentID);
    Task<IReadOnlyCollection<StudentDTO>> ReadAllAsync();
}