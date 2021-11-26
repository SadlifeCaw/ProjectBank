public interface ITagRepository 
{
    Task<TagDTO> CreateAsync(ProgramCreateDTO program);
    Task<TagDTO> ReadFacultyByIDAsync(int programID);
    Task<IReadOnlyCollection<TagDTO>> ReadAllAsync();
}