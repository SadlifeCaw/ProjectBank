namespace ProjectBank.Core.EF.Repository;

public interface ITagRepository
{
    Task<(Response, TagDTO)> CreateAsync(TagCreateDTO program);
    Task<TagDTO> ReadTagByIDAsync(int TagID);

    Task<TagDTO> ReadTagByNameAsync(string TagName);
    Task<IReadOnlyCollection<TagDTO>> ReadAllAsync();

    Task<IReadOnlyCollection<TagDTO>> ReadCollectionAsync(ICollection<int> tagIDs);
}