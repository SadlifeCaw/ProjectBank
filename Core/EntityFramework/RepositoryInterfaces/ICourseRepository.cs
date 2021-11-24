namespace ProjectBank.Core.EF.Repository;

public interface ICourseRepository 
{
    Task<CourseDTO> CreateAsync(CourseCreateDTO course);
    Task<Option<CourseDTO>> ReadAsync(int courseID);
    Task<IReadOnlyCollection<CourseDTO>> ReadAsync();
}