namespace ProjectBank.Core.EF.Repository;

public interface ICourseRepository 
{
    Task<CourseDTO> CreateAsync(CourseCreateDTO course);
    Task<CourseDTO> ReadCourseByIDAsync(int courseID);
    Task<IReadOnlyCollection<CourseDTO>> ReadAllAsync();
}