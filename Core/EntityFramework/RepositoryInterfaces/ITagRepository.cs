namespace ProjectBank.Core.EF.Repository;

public interface ITagRepository
{
    Task<(Response, TagDTO)> CreateAsync(TagCreateDTO program);
    Task<TagDTO> ReadTagByIDAsync(int TagID);
    Task<IReadOnlyCollection<TagDTO>> ReadAllAsync();
}